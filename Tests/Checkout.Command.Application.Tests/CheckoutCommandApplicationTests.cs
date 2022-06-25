using Checkout.Command.Application.Dtos;
using Checkout.Domain.Transaction.ValueObjects;
using MediatR;
using Moq;
using NUnit.Framework;

namespace Checkout.Command.Application.Tests;

[TestFixture]
internal class CheckoutCommandApplicationTests
{
    private Mock<ISender> _sender;
    private CheckoutCommandApplication _application;

    [SetUp]
    public void Setup()
    {
        _sender = new Mock<ISender>();
        _application = new CheckoutCommandApplication(_sender.Object);
    }

    internal class SubmitPayment : CheckoutCommandApplicationTests
    {
        [Test]
        public async Task Given_Input_Parameters_Then_An_SubmitPayment_Command_Should_Be_Sent()
        {
            //Act
            _ = await _application.SubmitPayment(Guid.NewGuid(), new CardDetailsDto(), 100);

            //Assert
            _sender.Verify(mock => mock.Send(It.IsAny<Application.Commands.SubmitPayment>(), default), Times.Exactly(1));
        }
    }
}
