using Checkout.Command.Application.Commands;
using Checkout.Command.Application.Dtos;
using Checkout.Command.Application.Events;
using Checkout.Command.Application.Interfaces;
using Checkout.Domain.Transaction;
using Checkout.Domain.Transaction.Exceptions;
using FluentAssertions;
using MediatR;
using Moq;
using NUnit.Framework;

namespace Checkout.Command.Application.Tests.Commands;

[TestFixture]
internal class SubmitPaymentTests
{
    private Mock<ITransactionsWriteRepository> _transactionsWriteRepository;
    private Mock<IPublisher> _publisher;
    private SubmitPaymenthandler _submitPaymentHandler;

    [SetUp]
    public void Setup()
    {
        _transactionsWriteRepository = new Mock<ITransactionsWriteRepository>();
        _publisher = new Mock<IPublisher>();
        _submitPaymentHandler = new SubmitPaymenthandler(_transactionsWriteRepository.Object, _publisher.Object); 
    }

    [Test]
    public void Given_An_Invalid_SubmitPayment_Command_Then_The_Transaction_Should_Not_Be_Stored()
    {
        //Arrange
        var command = new SubmitPayment(Guid.NewGuid(), new CardDetailsDto
        {
            HolderName = "Paolo Regoli",
            Number = "",
            Cvv = "123",
            ExpirationMonth = "12",
            ExpirationYear = "2026"
        }, 100);

        //Act
        Func<Task> action = async () => await _submitPaymentHandler.Handle(command, default);

        //Assert
        _ = action.Should().ThrowAsync<InvalidCardException>();
        _transactionsWriteRepository.Verify(mock => mock.SaveAsync(It.IsAny<Transaction>()), Times.Never);
        _publisher.Verify(mock => mock.Publish(It.IsAny<PaymentSubmitted>(), default), Times.Never);
    }
    
    [Test]
    public async Task Given_A_Valid_SubmitPayment_Command_Then_The_Transaction_Should_Be_Stored()
    {
        //Arrange
        var command = new SubmitPayment(Guid.NewGuid(), new CardDetailsDto
        {
            HolderName = "Paolo Regoli",
            Number = "4242424242424242",
            Cvv = "100",
            ExpirationMonth = "12",
            ExpirationYear = "2026"
        }, 100);

        //Act
        var transactionId = await _submitPaymentHandler.Handle(command, default);

        //Act & Assert
        transactionId.Should().NotBe(Guid.Empty);
        _transactionsWriteRepository.Verify(mock => mock.SaveAsync(It.IsAny<Transaction>()), Times.Exactly(1));
        _publisher.Verify(mock => mock.Publish(It.IsAny<PaymentSubmitted>(), default), Times.Exactly(1));
    }
}
