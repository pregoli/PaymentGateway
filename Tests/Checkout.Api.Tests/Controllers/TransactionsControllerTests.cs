using Checkout.Api.Controllers;
using Checkout.Api.Requests;
using Checkout.Command.Application.Interfaces;
using Checkout.Domain.Transaction.ValueObjects;
using Checkout.Query.Application.Interfaces;
using Moq;
using NUnit.Framework;

namespace Checkout.Api.Tests.Controllers;

[TestFixture]
internal class TransactionsControllerTests
{
    private Mock<ICheckoutCommandApplication> _checkoutCommandApplication;
    private Mock<ICheckoutQueryApplication> _checkoutQueryApplication;

    [SetUp]
    public void Setup()
    {
        _checkoutCommandApplication = new Mock<ICheckoutCommandApplication>();
        _checkoutQueryApplication = new Mock<ICheckoutQueryApplication>();
    }

    internal class ExecutePayment : TransactionsControllerTests
    {
        [Test]
        public async Task Given_A_Transaction_Request_Then_A_The_Command_Service_Should_Be_Invoked()
        {
            //Arrange
            var transactionrequest = new TransactionRequest();
            var controller = new TransactionsController(_checkoutCommandApplication.Object, _checkoutQueryApplication.Object);

            //Act
            _ = await controller.ExecutePayment(transactionrequest);

            //Assert
            _checkoutCommandApplication.Verify(mock => mock.ExecutePayment(It.IsAny<Guid>(), It.IsAny<CardDetails>(), It.IsAny<decimal>()), Times.Exactly(1));
        }
    }
    
    internal class GetTransactionById : TransactionsControllerTests
    {
        [Test]
        public async Task Given_A_Transaction_Id_Then_A_The_Query_Service_Should_Be_Invoked()
        {
            //Arrange
            var transactionId = Guid.NewGuid();
            var controller = new TransactionsController(_checkoutCommandApplication.Object, _checkoutQueryApplication.Object);

            //Act
            _ = await controller.GetTransactionById(transactionId);

            //Assert
            _checkoutQueryApplication.Verify(mock => mock.GetTransactionById(transactionId), Times.Exactly(1));
        }
    }
    
    internal class GetTransactionByMerchantId : TransactionsControllerTests
    {
        [Test]
        public async Task Given_A_Merchant_Id_Then_A_The_Query_Service_Should_Be_Invoked()
        {
            //Arrange
            var merchantId = Guid.NewGuid();
            var controller = new TransactionsController(_checkoutCommandApplication.Object, _checkoutQueryApplication.Object);

            //Act
            _ = await controller.GetTransactionByMerchantId(merchantId);

            //Assert
            _checkoutQueryApplication.Verify(mock => mock.GetTransactionsByMerchantId(merchantId), Times.Exactly(1));
        }
    }
}
