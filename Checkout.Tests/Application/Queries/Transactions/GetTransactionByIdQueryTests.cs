using Checkout.Application.Queries.Transactions;
using Checkout.Command.Application.Common.Dto;
using Checkout.Command.Application.Common.Interfaces;
using Checkout.Query.Application.Common.Dto;
using Checkout.Query.Application.Common.Interfaces;
using Checkout.Tests.Mock;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Checkout.Tests.Application.Queries.Transactions
{
    [TestFixture]
    public class GetTransactionByIdQueryTests
    {
        private GetTransactionById _request;
        private GetTransactionByIdQueryHandler _handler;

        private Mock<ICardsService> _cardsService;
        private Mock<ITransactionsHistoryService> _transactionsHistoryService;
        private Mock<LoggerMock<GetTransactionByIdQueryHandler>> _logger;

        [SetUp]
        public void Setup()
        {
            _cardsService = new Mock<ICardsService>();
            _transactionsHistoryService = new Mock<ITransactionsHistoryService>();
            _logger = new Mock<LoggerMock<GetTransactionByIdQueryHandler>>();

            _request = new GetTransactionById();
            _handler = new GetTransactionByIdQueryHandler(
                _cardsService.Object,
                _transactionsHistoryService.Object,
                _logger.Object);
        }

        [Test]
        public async Task Should_Contain_ServiceUnavailable_StatusCode_When_An_Exception_Throws_By_The_Service()
        {
            //Arange
            _transactionsHistoryService.Setup(x => x.GetByTransactionIdAsync(It.IsAny<Guid>()))
                .Throws(new Exception());

            //Act
            var result = await _handler.Handle(_request, new CancellationToken());

            //Assert
            _logger.Verify(x => x.Log(LogLevel.Error, It.IsAny<Exception>(), It.IsAny<string>()), Times.Once);
            Assert.NotNull(result);
            Assert.AreEqual(result.StatusCode, HttpStatusCode.ServiceUnavailable.ToString());
        }

        [Test]
        public async Task Should_Contain_NotFound_StatusCode_When_A_Transaction_Could_Not_Be_Found_By_The_Service()
        {
            //Arange
            TransactionItemDto response = null;
            _transactionsHistoryService.Setup(x => x.GetByTransactionIdAsync(It.IsAny<Guid>()))
                .Returns(Task.FromResult(response));

            //Act
            var result = await _handler.Handle(_request, new CancellationToken());

            //Assert
            _logger.Verify(x => x.Log(LogLevel.Error, It.IsAny<Exception>(), It.IsAny<string>()), Times.Never);
            Assert.NotNull(result);
            Assert.AreEqual(result.StatusCode, HttpStatusCode.NotFound.ToString());
        }

        [Test]
        public async Task Should_Contain_Successful_StatusCode_When_A_Successfully_Transaction_Could_Be_Found_By_The_Service()
        {
            //Arange
            var transactionId = Guid.NewGuid();
            var merchantId = Guid.NewGuid();
            var response = new TransactionItemDto(
                transactionId,
                merchantId,
                100,
                "Paolo Regoli",
                "1234567812345678",
                "Successful",
                string.Empty);

            _request.Id = transactionId;

            _transactionsHistoryService.Setup(x => x.GetByTransactionIdAsync(_request.Id))
                .Returns(Task.FromResult(response));

            _cardsService.Setup(x => x.Decrypt(response.CardNumber)).Returns(response.CardNumber);

            //Act
            var result = await _handler.Handle(_request, new CancellationToken());

            //Assert
            _logger.Verify(x => x.Log(LogLevel.Error, It.IsAny<Exception>(), It.IsAny<string>()), Times.Never);
            Assert.NotNull(result);
            Assert.AreEqual(result.StatusCode, "Successful");
        }
    }
}
