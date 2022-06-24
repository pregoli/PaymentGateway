using Checkout.Command.Application.Dtos;
using Checkout.Command.Application.Events;
using Checkout.Command.Application.Interfaces;
using Checkout.Domain.Transaction;
using Checkout.Domain.Transaction.ValueObjects;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace Checkout.Command.Application.Tests.Events;

[TestFixture]
internal class PaymentExecutedTests
{
    private Mock<IAcquiringBankProvider> _acquiringBankProvider;
    private Mock<ITransactionsHistoryCommandRepository> _transactionsHistoryCommandRepository;
    private Mock<ILogger<PaymentExecutedHandler>> _logger;
    private PaymentExecutedHandler _paymentExecutedHandler;

    private readonly Transaction transactionPayload = Transaction.Create(Guid.NewGuid(), 100, new CardDetails
    {
        CardHolderName = "Paolo Regoli",
        CardNumber = "4242424242424242",
        Cvv = "100",
        ExpirationMonth = "12",
        ExpirationYear = "2026"
    });

    [SetUp]
    public void Setup()
    {
        _acquiringBankProvider = new Mock<IAcquiringBankProvider>();
        _transactionsHistoryCommandRepository = new Mock<ITransactionsHistoryCommandRepository>();
        _logger = new Mock<ILogger<PaymentExecutedHandler>>();
        _paymentExecutedHandler = new PaymentExecutedHandler(_acquiringBankProvider.Object, _transactionsHistoryCommandRepository.Object, default);
    }

    [Test]
    public async Task Given_A_PaymentExecuted_Event_With_An_Invalid_Payload_Then_The_Transaction_Should_Be_Rejected()
    {
        //Arrange
        var @event = new PaymentExecuted(transactionPayload);

        _ = _acquiringBankProvider.Setup(x => x.ValidateTransaction(It.IsAny<TransactionAuthorizationRequest>()))
            .Returns(new TransactionAuthorizationResponse(default, Authorized: false, default, default));

        //Act
        await _paymentExecutedHandler.Handle(@event, default);

        //Assert
        Assert.False(transactionPayload.Successful);
        _transactionsHistoryCommandRepository.Verify(mock => mock.UpdateAsync(transactionPayload), Times.Once);
    }

    [Test]
    public async Task Given_A_PaymentExecuted_Event_With_A_Valid_Payload_Then_The_Transaction_Should_Be_Authorized()
    {
        //Arrange
        var @event = new PaymentExecuted(transactionPayload);

        _ = _acquiringBankProvider.Setup(x => x.ValidateTransaction(It.IsAny<TransactionAuthorizationRequest>()))
            .Returns(new TransactionAuthorizationResponse(default, Authorized: true, default, default));

        //Act
        await _paymentExecutedHandler.Handle(@event, default);

        //Assert
        Assert.True(transactionPayload.Successful);
        _transactionsHistoryCommandRepository.Verify(mock => mock.UpdateAsync(transactionPayload), Times.Once);
    }
}
