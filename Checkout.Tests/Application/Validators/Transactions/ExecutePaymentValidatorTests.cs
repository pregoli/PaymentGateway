using Checkout.Command.Application.Common.Dto;
using Checkout.Command.Application.Validators.Transactions;
using Checkout.Query.Application.Common.Dto;
using FluentValidation.TestHelper;
using NUnit.Framework;
using System;

namespace Checkout.Tests.Application.Validators.Transactions
{
    [TestFixture]
    public class ExecutePaymentValidatorTests
    {
        private CardDetails cardDetails;
        private ExecutePaymentValidator validator;

        [SetUp]
        public void Setup() {
           validator = new ExecutePaymentValidator();
        }

        [TestCase(0)]
        [TestCase(-10)]
        public void Should_Have_Error_When_Amount_Less_Than_Or_Equal_To_Zero(decimal amount)
        {
            validator.ShouldHaveValidationErrorFor(command => command.Amount, amount); 
        }

        [TestCase(null)]
        [TestCase("00000000-0000-0000-0000-000000000000")]
        public void Should_Have_Error_When_MerchantId_Is_Empty_Or_Null(Guid merchantId)
        {
            validator.ShouldHaveValidationErrorFor(command => command.MerchantId, merchantId); 
        }

        [Test]
        public void Should_Have_Error_When_CardDetails_Is_Null()
        {
            cardDetails = null;
            validator.ShouldHaveValidationErrorFor(command => command.CardDetails, cardDetails); 
        }
    }
}
