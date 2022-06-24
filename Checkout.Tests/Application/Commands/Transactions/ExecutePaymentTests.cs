using Checkout.Application.Commands.Transactions;
using Checkout.Application.Common.Dto;
using Checkout.Command.Application.Common.Dto;
using Checkout.Command.Application.Common.Interfaces;
using Checkout.Query.Application.Common.Dto;
using Checkout.Query.Application.Common.Interfaces;
using Checkout.Tests.Mock;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Checkout.Tests.Application.Commands.Transactions
{
    [TestFixture]
    public class ExecutePaymentTests
    {
        private ExecutePayment _command;
        private ExecutePaymentHandler _handler;

        private Mock<ICardsService> _cardsService;
        private Mock<IBankAuthProvider> _bankAuthProvider;
        private Mock<IMediator> _mediator;
        private Mock<LoggerMock<ExecutePaymentHandler>> _logger;

        [SetUp]
        public void Setup()
        {
            _cardsService = new Mock<ICardsService>();
            _bankAuthProvider = new Mock<IBankAuthProvider>();
            _mediator = new Mock<IMediator>();
            _logger = new Mock<LoggerMock<ExecutePaymentHandler>>();

            _command = new ExecutePayment
            {
                Amount = 100,
                CardDetails = new CardDetails(),
                MerchantId = Guid.NewGuid()
            };

            _handler = new ExecutePaymentHandler(
                _cardsService.Object,
                _bankAuthProvider.Object,
                _mediator.Object,
                _logger.Object);
        }

        [Test]
        public async Task Should_Return_A_TransactionId_When_An_Exception_Thrown_By_AuthProvider()
        {
            //Arange
            _cardsService.Setup(x => x.Validate(It.IsAny<CardDetails>())).Returns(true);
            _bankAuthProvider.Setup(x => x.VerifyAsync(It.IsAny<TransactionRequest>()))
                .Throws(new Exception());

            //Act
            var result = await _handler.Handle(_command, new CancellationToken());

            //Assert
            _logger.Verify(x => x.Log(LogLevel.Error, It.IsAny<Exception>(), It.IsAny<string>()), Times.Once);
            Assert.NotNull(result);
            Assert.AreNotEqual(result, Guid.Empty);
        }

        [Test]
        public async Task Should_Return_A_TransactionId_When_An_Invalid_Card_Data()
        {
            //Arange
            _cardsService.Setup(x => x.Validate(It.IsAny<CardDetails>())).Returns(false);

            //Act
            var result = await _handler.Handle(_command, new CancellationToken());

            //Assert
            _bankAuthProvider.Verify(x => x.VerifyAsync(It.IsAny<TransactionRequest>()), Times.Never);
            Assert.NotNull(result);
            Assert.AreNotEqual(result, Guid.Empty);
        }

        [TestCase("20150", "Card not 3D Secure enabled", false)]
        [TestCase("20153", "3D Secure system malfunction", false)]
        [TestCase("20154", "3D Secure Authentication Required", false)]
        [TestCase("Successful", "", true)]
        public async Task Should_Return_A_TransactionId_When_The_Auth_Verify_The_Transaction(
            string statusCode, string description, bool verified)
        {
            //Arange
            _cardsService.Setup(x => x.Validate(It.IsAny<CardDetails>())).Returns(true);

            var transactionId = Guid.NewGuid();
            var transactionAuthResponse = new TransactionResponse(transactionId, verified, statusCode, description);
            _bankAuthProvider.Setup(x => x.VerifyAsync(It.IsAny<TransactionRequest>()))
                .Returns(Task.FromResult(transactionAuthResponse));
            //Act
            var result = await _handler.Handle(_command, new CancellationToken());

            //Assert
            _bankAuthProvider.Verify(x => x.VerifyAsync(It.IsAny<TransactionRequest>()), Times.Exactly(1));
            Assert.NotNull(result);
            Assert.AreNotEqual(result, Guid.Empty);
        }
    }
}
