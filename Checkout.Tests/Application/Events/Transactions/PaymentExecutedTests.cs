using Checkout.Application.Events.Transactions;
using Checkout.Command.Application.Common.Dto;
using Checkout.Command.Application.Common.Interfaces;
using Checkout.Query.Application.Common.Dto;
using Checkout.Query.Application.Common.Interfaces;
using Checkout.Tests.Mock;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Checkout.Tests.Application.Events.Transactions
{
    [TestFixture]
    public class PaymentExecutedTests
    {
        private PaymentExecuted _event;
        private PaymentExecutedHandler _handler;

        private Mock<ICardsService> _cardsService;
        private Mock<ITransactionsHistoryService> _transactionsHistoryService;
        private Mock<LoggerMock<PaymentExecutedHandler>> _logger;

        [SetUp]
        public void Setup()
        {
            _cardsService = new Mock<ICardsService>();
            _transactionsHistoryService = new Mock<ITransactionsHistoryService>();
            _logger = new Mock<LoggerMock<PaymentExecutedHandler>>();

            _event = new PaymentExecuted
                (
                Guid.NewGuid(),
                Guid.NewGuid(),
                100,
                "Paolo Regoli",
                "2222333344445555",
                "Successful",
                string.Empty,
                true);

            _handler = new PaymentExecutedHandler(
                _transactionsHistoryService.Object,
                _cardsService.Object,
                _logger.Object);
        }

        [Test]
        public async Task Should_Log_An_Error_When_An_Exception_Throws()
        {
            //Arange
            _transactionsHistoryService.Setup(x => x.AddAsync(It.IsAny<TransactionItemDto>()))
                .Throws(new Exception("test"));

            //Act + Assert
            var ex = Assert.ThrowsAsync<Exception>(() => _handler.Handle(_event, new CancellationToken()));
            Assert.That(ex.Message, Is.EqualTo("test"));
            _logger.Verify(x => x.Log(LogLevel.Error, It.IsAny<Exception>(), It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task Should_Not_Throw_An_Exception_When_The_Service_Return_A_Response()
        {
            //Arange
            _transactionsHistoryService.Setup(x => x.AddAsync(It.IsAny<TransactionItemDto>()))
                .Returns(Task.FromResult(new TransactionItemDto()));

            //Act
            await _handler.Handle(_event, new CancellationToken());

            //Assert
            _logger.Verify(x => x.Log(LogLevel.Error, It.IsAny<Exception>(), It.IsAny<string>()), Times.Never);
        }
    }
}
