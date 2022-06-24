using Checkout.Application.Common.Dto;
using Checkout.Application.Services;
using Checkout.Command.Application.Common.Interfaces;
using Checkout.Query.Application.Common.Interfaces;
using NUnit.Framework;

namespace Checkout.Tests.Application.Services
{
    [TestFixture]
    public class CardsServiceTests
    {
        private ICardsService _cardService;
        [OneTimeSetUp]
        public void Setup()
        { 
            _cardService = new CardsService();
        }

        [TestCase(null)]
        [TestCase("")]
        public void Validation_Should_Fail_For_A_Null_Or_Empty_Card_Number(string cardNumber)
        { 
            //Arrange
            var cardDetails = new CardDetails
            { 
                CardNumber = cardNumber
            };

            //Act
            var result = _cardService.Validate(cardDetails);

            //Assert
            Assert.False(result);
        }
        
        [TestCase("hello")]
        [TestCase("123412341234")]
        public void Validation_Should_Fail_For_An_Invalid_Card_Number(string cardNumber)
        { 
            //Arrange
            var cardDetails = new CardDetails
            { 
                CardNumber = cardNumber
            };

            //Act
            var result = _cardService.Validate(cardDetails);

            //Assert
            Assert.False(result);
        }
    }
}
