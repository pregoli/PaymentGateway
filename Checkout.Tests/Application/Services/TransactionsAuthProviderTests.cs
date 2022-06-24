using Checkout.Application.Common.Dto;
using Checkout.Command.Application.Services;
using Checkout.Domain.Entities;
using Checkout.Infrastructure.Persistence.Repositories;
using Checkout.Tests.Mock;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Checkout.Tests.Application.Services
{
    [TestFixture]
    public class TransactionsAuthProviderTests
    {
        private TransactionRequest _transactionAuthRequest;
        private DevBankAuthProvider _devBankAuthProvider;

        private Mock<IDevBankAuthRepository> _bankAuthProvider;
        private Mock<LoggerMock<DevBankAuthProvider>> _logger;

        [SetUp]
        public void Setup()
        {
            _bankAuthProvider = new Mock<IDevBankAuthRepository>();
            _logger = new Mock<LoggerMock<DevBankAuthProvider>>();

            _transactionAuthRequest = new TransactionRequest(new CardDetails(), 100);
            _devBankAuthProvider = new DevBankAuthProvider(
                _bankAuthProvider.Object,
                _logger.Object);
        }

        [Test]
        public async Task For_A_Given_Zero_Amount_A_NotAcceptable_StatusCode_From_Response_Is_Expected()
        {
            //Arange
            _transactionAuthRequest.Amount = 0;

            //Act
            var result = await _devBankAuthProvider.VerifyAsync(_transactionAuthRequest);

            //Assert
            Assert.NotNull(result);
            Assert.AreEqual(result.Code, HttpStatusCode.NotAcceptable.ToString());
        }

        [Test]
        public async Task For_A_Given_Exception_Thrown_By_Repository_A_ServiceUnavailable_StatusCode_From_Response_Is_Expected()
        {
            //Arange
            _transactionAuthRequest.Amount = 100;

            _bankAuthProvider.Setup(x => x.ValidateTransaction(It.IsAny<decimal>()))
                .Throws(new Exception());

            //Act
            var result = await _devBankAuthProvider.VerifyAsync(_transactionAuthRequest);

            //Assert
            _logger.Verify(x => x.Log(LogLevel.Error, It.IsAny<Exception>(), It.IsAny<string>()), Times.Once);
            Assert.NotNull(result);
            Assert.AreEqual(result.Code, HttpStatusCode.ServiceUnavailable.ToString());
        }

        [Test]
        public async Task Should_Return_A_Successful_StatusCode_When_A_Null_Response_By_Repository()
        {
            //Arange
            _transactionAuthRequest.Amount = 100;

            Transaction response = null;
            _bankAuthProvider.Setup(x => x.ValidateTransaction(It.IsAny<decimal>()))
                .Returns(Task.FromResult(response));

            //Act
            var result = await _devBankAuthProvider.VerifyAsync(_transactionAuthRequest);

            //Assert
            Assert.NotNull(result);
            Assert.AreEqual(result.Code, "10000");
        }
        
        [Test]
        public async Task Should_Return_A_Not_Successful_StatusCode_When_A_Not_Null_Null_Response_By_Repository()
        {
            //Arange
            _transactionAuthRequest.Amount = 105;

            var response = new Transaction(Guid.NewGuid(),"05", "20005", "Declined - Do not honour");
            _bankAuthProvider.Setup(x => x.ValidateTransaction(It.IsAny<decimal>()))
                .Returns(Task.FromResult(response));

            //Act
            var result = await _devBankAuthProvider.VerifyAsync(_transactionAuthRequest);

            //Assert
            Assert.NotNull(result);
            Assert.AreNotEqual(result.Code, "10000");
            Assert.AreEqual(result.Code, response.TransactionCode);
        }
    }
}
