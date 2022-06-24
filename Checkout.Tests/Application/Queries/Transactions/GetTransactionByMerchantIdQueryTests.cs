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
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Checkout.Tests.Application.Queries.Transactions
{
    [TestFixture]
    public class GetTransactionByMerchantIdQueryTests
    {
        private GetTransactionByMerchantId _request;
        private GetTransactionByMerchantIdQueryHandler _handler;

        private Mock<ICardsService> _cardsService;
        private Mock<ITransactionsHistoryService> _transactionsHistoryService;
        private Mock<LoggerMock<GetTransactionByMerchantIdQueryHandler>> _logger;

        [SetUp]
        public void Setup()
        {
            _cardsService = new Mock<ICardsService>();
            _transactionsHistoryService = new Mock<ITransactionsHistoryService>();
            _logger = new Mock<LoggerMock<GetTransactionByMerchantIdQueryHandler>>();

            _request = new GetTransactionByMerchantId();
            _handler = new GetTransactionByMerchantIdQueryHandler(
                _cardsService.Object,
                _transactionsHistoryService.Object,
                _logger.Object);
        }

        [Test]
        public async Task Should_Contain_Empty_Response_When_An_Exception_Throws_By_Service_By_The_Service()
        {
            //Arange
            _transactionsHistoryService.Setup(x => x.GetByMerchantIdAsync(It.IsAny<Guid>()))
                .Throws(new Exception());

            //Act
            var result = await _handler.Handle(_request, new CancellationToken());

            //Assert
            _logger.Verify(x => x.Log(LogLevel.Error, It.IsAny<Exception>(), It.IsAny<string>()), Times.Once);
            Assert.NotNull(result);
            Assert.IsEmpty(result);
        }

        [Test]
        public async Task Should_Contain_Empty_Response_When_Any_Transaction_Could_Be_Found_By_The_Given_MerchantId()
        {
            //Arange
            List<TransactionItemDto> response = null;
            _transactionsHistoryService.Setup(x => x.GetByMerchantIdAsync(It.IsAny<Guid>()))
                .Returns(Task.FromResult(response));

            //Act
            var result = await _handler.Handle(_request, new CancellationToken());

            //Assert
            _logger.Verify(x => x.Log(LogLevel.Error, It.IsAny<Exception>(), It.IsAny<string>()), Times.Never);
            Assert.NotNull(result);
            Assert.IsEmpty(result);
        }
        
        [Test]
        public async Task Should_Contain_A_Not_Empty_When_At_Least_One_Transaction_Could_Be_Found_By_The_Given_MerchantId()
        {
            //Arange
            var transactionId = Guid.NewGuid();
            var merchantId = Guid.NewGuid();
            var response = new List<TransactionItemDto> 
            {
                new TransactionItemDto(
                    transactionId,
                    merchantId,
                    100,
                    "Paolo Regoli",
                    "1234567812345678",
                    "Successful",
                    string.Empty) 
            };

            _request.MerchantId = merchantId;

            _transactionsHistoryService.Setup(x => x.GetByMerchantIdAsync(_request.MerchantId))
                .Returns(Task.FromResult(response));

            _cardsService.Setup(x => x.Decrypt(It.IsAny<string>())).Returns("1234567812345678");

            //Act
            var result = await _handler.Handle(_request, new CancellationToken());

            //Assert
            _logger.Verify(x => x.Log(LogLevel.Error, It.IsAny<Exception>(), It.IsAny<string>()), Times.Never);
            Assert.NotNull(result);
            Assert.AreEqual(result.Count, 1);
            Assert.AreEqual(result.FirstOrDefault().TransactionId, transactionId);
        }
    }
}
