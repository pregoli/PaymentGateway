using Checkout.Api.Controllers;
using Checkout.Api.Requests;
using Checkout.Command.Application.Dtos;
using Checkout.Command.Application.Interfaces;
using Moq;
using NUnit.Framework;

namespace Checkout.Api.Tests.Controllers;

[TestFixture]
internal class PaymentsControllerTests
{
    private Mock<ICheckoutCommandApplication> _checkoutCommandApplication = null!;
    private PaymentsController _controller = null!;

    [SetUp]
    public void Setup()
    {
        _checkoutCommandApplication = new Mock<ICheckoutCommandApplication>();
        _controller = new PaymentsController(_checkoutCommandApplication.Object);
    }

    internal class SubmitPayment : PaymentsControllerTests
    {
        [Test]
        public async Task Given_A_Transaction_Request_Then_The_Command_Service_Should_Be_Invoked()
        {
            //Arrange
            var transactionrequest = new TransactionRequest();

            //Act
            _ = await _controller.SubmitPayment(transactionrequest);

            //Assert
            _checkoutCommandApplication.Verify(mock => mock.SubmitPayment(It.IsAny<Guid>(), It.IsAny<CardDetailsDto>(), It.IsAny<decimal>()), Times.Exactly(1));
        }
    }
}