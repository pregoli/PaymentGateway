using Checkout.Api.Controllers;
using Checkout.Application.Commands.Transactions;
using Checkout.Application.Queries.Transactions;
using Checkout.Command.Application.Common.ViewModels;
using Checkout.Query.Application.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Checkout.Tests.Api.Controllers
{
    [TestFixture]
    public class TransactionsControllerTests
    {
        ExecutePayment _command;
        private TransactionsController _controller;
        private Mock<IMediator> _mediator;

        [SetUp]
        public void Setup()
        {
            _mediator = new Mock<IMediator>();
            _controller = new TransactionsController(_mediator.Object);
        }

        [Test]
        public async Task Should_Be_Redirected_To_The_GetTransactionById_On_Completion()
        {
            //Arange
            var transactionId = Guid.NewGuid();

            _mediator.Setup(x => x.Send(It.IsAny<ExecutePayment>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(transactionId));

            //Act
            var result = await _controller.ExecutePayment(It.IsAny<ExecutePayment>());

            //Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<RedirectToRouteResult>(result);
            Assert.AreEqual(((RedirectToRouteResult)result).RouteName, "GetTransactionById");
            Assert.AreEqual(((RedirectToRouteResult)result).RouteValues["id"], transactionId);
        }

        [Test]
        public async Task Should_Return_A_TransactionResponseVm()
        {
            //Arange
            var transactionId = Guid.NewGuid();

            _mediator.Setup(x => x.Send(It.IsAny<GetTransactionById>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new TransactionHistoryResponse (
                            transactionId: transactionId,
                            merchantId: Guid.Empty,
                            cardHolderName: string.Empty,
                            cardNumber: string.Empty,
                            amount: 0,
                            statusCode: HttpStatusCode.ServiceUnavailable.ToString(),
                            description: "Unfortunately It was not possible to process your request",
                            timestamp: DateTime.MinValue)));

            //Act
            var result = await _controller.GetTransactionById(transactionId);

            //Assert
            Assert.NotNull(result.Value);
            Assert.IsAssignableFrom<TransactionHistoryResponse>(result.Value);
            Assert.AreEqual(result.Value.TransactionId, transactionId);
        }

        [Test]
        public async Task Should_Return_A_Collection_Of_TransactionResponseVm()
        {
            //Arange
            var transactionId = Guid.NewGuid();

            _mediator.Setup(x => x.Send(It.IsAny<GetTransactionByMerchantId>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new List<TransactionHistoryResponse> {new TransactionHistoryResponse (
                            transactionId: transactionId,
                            merchantId: Guid.Empty,
                            cardHolderName: string.Empty,
                            cardNumber: string.Empty,
                            amount: 0,
                            statusCode: HttpStatusCode.ServiceUnavailable.ToString(),
                            description: "Unfortunately It was not possible to process your request",
                            timestamp: DateTime.MinValue) }));

            //Act
            var result = await _controller.GetTransactionByMerchantId(transactionId);

            //Assert
            Assert.NotNull(result.Value);
            Assert.IsAssignableFrom<List<TransactionHistoryResponse>>(result.Value);
            Assert.AreEqual(result.Value[0].TransactionId, transactionId);
        }
    }
}
