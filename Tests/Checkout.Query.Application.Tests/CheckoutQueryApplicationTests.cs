using MediatR;
using Moq;
using NUnit.Framework;

namespace Checkout.Query.Application.Tests;

[TestFixture]
internal class CheckoutQueryApplicationTests
{
    private Mock<ISender> _sender;
    private CheckoutQueryApplication _application;

    [SetUp]
    public void Setup()
    {
        _sender = new Mock<ISender>();
        _application = new CheckoutQueryApplication(_sender.Object);
    }

    internal class GetTransactionById : CheckoutQueryApplicationTests
    {
        [Test]
        public async Task Given_A_Transaction_Id_Then_A_GetTransactionById_Query_Command_Should_Be_Sent()
        {
            //Assert
            var query = new Application.Queries.GetTransactionById(Guid.NewGuid());

            //Act
            _ = await _application.GetTransactionById(query.Id);

            //Assert
            _sender.Verify(mock => mock.Send(It.IsAny<Application.Queries.GetTransactionById>(), default), Times.Exactly(1));
        }
    }
    
    internal class GetTransactionsByMerchantId : CheckoutQueryApplicationTests
    {
        [Test]
        public async Task Given_A_Merchant_Id_Then_A_GetTransactionsByMerchantId_Query_Command_Should_Be_Sent()
        {
            //Assert
            var query = new Application.Queries.GetTransactionsByMerchantId(Guid.NewGuid());

            //Act
            _ = await _application.GetTransactionsByMerchantId(query.MerchantId);

            //Assert
            _sender.Verify(mock => mock.Send(It.IsAny<Application.Queries.GetTransactionsByMerchantId>(), default), Times.Exactly(1));
        }
    }
}
