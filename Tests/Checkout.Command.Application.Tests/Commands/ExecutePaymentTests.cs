using Checkout.Command.Application.Commands;
using Checkout.Command.Application.Events;
using Checkout.Command.Application.Interfaces;
using Checkout.Domain.Transaction;
using Checkout.Domain.Transaction.Exceptions;
using Checkout.Domain.Transaction.ValueObjects;
using MediatR;
using Moq;
using NUnit.Framework;

namespace Checkout.Command.Application.Tests.Commands;

[TestFixture]
internal class ExecutePaymentTests
{
    private Mock<ITransactionsHistoryCommandRepository> _transactionsHistoryCommandRepository;
    private Mock<IPublisher> _publisher;
    private ExecutePaymentHandler _executePaymentHandler;

    [SetUp]
    public void Setup()
    {
        _transactionsHistoryCommandRepository = new Mock<ITransactionsHistoryCommandRepository>();
        _publisher = new Mock<IPublisher>();
        _executePaymentHandler = new ExecutePaymentHandler(_transactionsHistoryCommandRepository.Object, _publisher.Object); 
    }

    [Test]
    public async Task Given_An_Invalid_ExecutePayment_Command_Then_The_Transaction_Should_Not_Be_Stored()
    {
        //Arrange
        var command = new ExecutePayment(Guid.NewGuid(), new CardDetails
        {
            CardHolderName = "Paolo Regoli",
            CardNumber = "",
            Cvv = "123",
            ExpirationMonth = "12",
            ExpirationYear = "2026"
        }, 100);

        //Act & Assert
        Assert.ThrowsAsync<InvalidCardException>(() => _executePaymentHandler.Handle(command, default));
        _transactionsHistoryCommandRepository.Verify(mock => mock.SaveAsync(It.IsAny<Transaction>()), Times.Never);
        _publisher.Verify(mock => mock.Publish(It.IsAny<PaymentExecuted>(), default), Times.Never);
    }
    
    [Test]
    public async Task Given_A_Valid_ExecutePayment_Command_Then_The_Transaction_Should_Be_Stored()
    {
        //Arrange
        var command = new ExecutePayment(Guid.NewGuid(), new CardDetails
        {
            CardHolderName = "Paolo Regoli",
            CardNumber = "4242424242424242",
            Cvv = "100",
            ExpirationMonth = "12",
            ExpirationYear = "2026"
        }, 100);

        //Act
        var transactionId = await _executePaymentHandler.Handle(command, default);

        //Act & Assert
        Assert.True(transactionId != Guid.Empty);
        _transactionsHistoryCommandRepository.Verify(mock => mock.SaveAsync(It.IsAny<Transaction>()), Times.Exactly(1));
        _publisher.Verify(mock => mock.Publish(It.IsAny<PaymentExecuted>(), default), Times.Exactly(1));
    }
}
