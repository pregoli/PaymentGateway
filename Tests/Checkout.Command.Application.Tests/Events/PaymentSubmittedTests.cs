using Checkout.Command.Application.Dtos;
using Checkout.Command.Application.Events;
using Checkout.Command.Application.Interfaces;
using Checkout.Domain.Transaction;
using Checkout.Domain.Transaction.ValueObjects;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace Checkout.Command.Application.Tests.Events;

[TestFixture]
internal class PaymentSubmittedTests
{
    private Mock<IAcquiringBankProvider> _acquiringBankProvider = null!;
    private Mock<ITransactionsWriteRepository> _transactionsWriteRepository = null!;
    private PaymentSubmittedHandler _paymentSubmittedHandler = null!;

    private readonly Transaction _transactionPayload = Transaction.Create(
        Guid.NewGuid(), 100, CardDetails.Create("Paolo Regoli", "4242424242424242", "12", "2026", "100"));

    [SetUp]
    public void Setup()
    {
        _acquiringBankProvider = new Mock<IAcquiringBankProvider>();
        _transactionsWriteRepository = new Mock<ITransactionsWriteRepository>();
        _paymentSubmittedHandler = new PaymentSubmittedHandler(_acquiringBankProvider.Object, _transactionsWriteRepository.Object, default!);
    }

    [Test]
    public async Task Given_A_PaymentSubmitted_Event_With_An_Invalid_Payload_Then_The_Transaction_Should_Be_Rejected()
    {
        //Arrange
        var @event = new PaymentSubmitted(_transactionPayload);

        _ = _acquiringBankProvider.Setup(x => x.ValidateTransaction(It.IsAny<TransactionAuthorizationRequest>()))
            .Returns(new TransactionAuthorizationResponse(default, Authorized: false, default!, default!));

        //Act
        await _paymentSubmittedHandler.Handle(@event, default);

        //Assert
        _transactionPayload!.Successful.Should().BeFalse();
        _transactionsWriteRepository.Verify(mock => mock.UpdateAsync(_transactionPayload), Times.Once);
    }

    [Test]
    public async Task Given_A_PaymentSubmitted_Event_With_A_Valid_Payload_Then_The_Transaction_Should_Be_Authorized()
    {
        //Arrange
        var @event = new PaymentSubmitted(_transactionPayload);

        _ = _acquiringBankProvider.Setup(x => x.ValidateTransaction(It.IsAny<TransactionAuthorizationRequest>()))
            .Returns(new TransactionAuthorizationResponse(default, Authorized: true, default!, default!));

        //Act
        await _paymentSubmittedHandler.Handle(@event, default);

        //Assert
        _transactionPayload!.Successful.Should().BeTrue();
        _transactionsWriteRepository.Verify(mock => mock.UpdateAsync(_transactionPayload), Times.Once);
    }
}
