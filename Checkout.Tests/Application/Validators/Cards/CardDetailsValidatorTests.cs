using Checkout.Command.Application.Validators.Cards;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace Checkout.Tests.Application.Validators.Cards
{
    [TestFixture]
    public class CardDetailsValidatorTests
    {
        private CardDetailsValidator validator;

        [SetUp]
        public void Setup() {
           validator = new CardDetailsValidator();
        }

        [TestCase(null)]
        [TestCase("")]
        public void Should_Have_Error_When_CardHolderName_Is_Null_Or_Empty(string cardHolderName) 
        {
            validator.ShouldHaveValidationErrorFor(x => x.CardHolderName, cardHolderName); 
        }
    
        [TestCase(null)]
        [TestCase("")]
        [TestCase("1234")]
        [TestCase("12")]
        [TestCase("hello")]
        public void Should_Have_Error_When_Cvv_Is_Null_Or_Empty_Or_Not_A_Number_Or_It_Is_Not_Compliant_With_The_Required_Length(string cvv) 
        {
            validator.ShouldHaveValidationErrorFor(x => x.Cvv, cvv);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("1234")]
        [TestCase("12")]
        [TestCase("hello")]
        public void Should_Have_Error_When_CardNumber_Is_Null_Or_Empty_Or_It_Is_Not_Compliant_With_The_Required_Length(string cardNumber) 
        {
            validator.ShouldHaveValidationErrorFor(x => x.CardNumber, cardNumber);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("111")]
        [TestCase("hello")]
        public void Should_Have_Error_When_ExpirationMonth_Is_Null_Or_Empty_Or_Not_A_Number_Or_It_Is_Not_Compliant_With_The_Required_Length(string expirationMonth) 
        {
            validator.ShouldHaveValidationErrorFor(x => x.ExpirationMonth, expirationMonth);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("111")]
        [TestCase("hello")]
        public void Should_Have_Error_When_ExpirationYear_Is_Null_Or_Empty_Or_Not_A_Number_Or_It_Is_Not_Compliant_With_The_Required_Length(string expirationYear) 
        {
            validator.ShouldHaveValidationErrorFor(x => x.ExpirationYear, expirationYear);
        }
    }
}
