﻿using System.Text.Json;
using Checkout.Api.Controllers;
using Checkout.Api.Requests;
using Checkout.Command.Application.Interfaces;
using Checkout.Domain.Transaction.ValueObjects;
using Checkout.Query.Application.Dtos;
using Checkout.Query.Application.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace Checkout.Api.Tests.Controllers;

[TestFixture]
internal class TransactionsControllerTests
{
    private Mock<ICheckoutCommandApplication> _checkoutCommandApplication = null!;
    private Mock<ICheckoutQueryApplication> _checkoutQueryApplication = null!;
    private TransactionsController _controller = null!;

    [SetUp]
    public void Setup()
    {
        _checkoutCommandApplication = new Mock<ICheckoutCommandApplication>();
        _checkoutQueryApplication = new Mock<ICheckoutQueryApplication>();
        _controller = new TransactionsController(_checkoutCommandApplication.Object, _checkoutQueryApplication.Object);
    }

    internal class ExecutePayment : TransactionsControllerTests
    {
        [Test]
        public async Task Given_A_Transaction_Request_Then_A_The_Command_Service_Should_Be_Invoked()
        {
            //Arrange
            var transactionrequest = new TransactionRequest();

            //Act
            _ = await _controller.ExecutePayment(transactionrequest);

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

            //Act
            _ = await _controller.GetTransactionById(transactionId);

            //Assert
            _checkoutQueryApplication.Verify(mock => mock.GetTransactionByIdAsync(transactionId), Times.Exactly(1));
        }
        
        [Test]
        public async Task Given_A_Transaction_Id_When_The_Transaction_Cannot_Be_Found_Then_A_404_StatusCode_Is_Expected()
        {
            //Arrange
            var transactionId = Guid.NewGuid();

            _ = _checkoutQueryApplication.Setup(mock => mock.GetTransactionByIdAsync(transactionId))!.ReturnsAsync((TransactionResponse)null!);

            //Act
            var result = (await _controller.GetTransactionById(transactionId)) as StatusCodeResult;

            //Assert
            result!.StatusCode.Should().Be(404);
            _checkoutQueryApplication.Verify(mock => mock.GetTransactionByIdAsync(transactionId), Times.Exactly(1));
        }
        
        [Test]
        public async Task Given_A_Transaction_Id_When_The_Transaction_Can_Be_Found_Then_A_200_StatusCode_Is_Expected()
        {
            //Arrange
            var transactionId = Guid.NewGuid();

            var cardDetails = new CardDetails
            {
                CardHolderName = "Paolo Regoli",
                CardNumber = "4242424242424242",
                Cvv = "100",
                ExpirationMonth = "12",
                ExpirationYear = "2026"
            };

            _ = _checkoutQueryApplication.Setup(mock => mock.GetTransactionByIdAsync(transactionId))!
                .ReturnsAsync(TransactionResponse.Map(transactionId, default, JsonSerializer.Serialize(cardDetails), default, default!, default!, default));

            //Act
            var result = (await _controller.GetTransactionById(transactionId)) as ObjectResult;

            //Assert
            result!.StatusCode.Should().Be(200);
            (result.Value as TransactionResponse)!.TransactionId.Should().Be(transactionId);
            _checkoutQueryApplication.Verify(mock => mock.GetTransactionByIdAsync(transactionId), Times.Exactly(1));
        }
    }
    
    internal class GetTransactionByMerchantId : TransactionsControllerTests
    {
        [Test]
        public async Task Given_A_Merchant_Id_Then_A_The_Query_Service_Should_Be_Invoked()
        {
            //Arrange
            var merchantId = Guid.NewGuid();

            //Act
            _ = await _controller.GetTransactionByMerchantId(merchantId);

            //Assert
            _checkoutQueryApplication.Verify(mock => mock.GetTransactionsByMerchantIdAsync(merchantId), Times.Exactly(1));
        }
    }
}
