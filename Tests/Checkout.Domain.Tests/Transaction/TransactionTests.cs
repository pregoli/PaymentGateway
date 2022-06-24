using System.Text.Json;
using Checkout.Domain.Transaction.Enums;
using Checkout.Domain.Transaction.Exceptions;
using Checkout.Domain.Transaction.ValueObjects;
using NUnit.Framework;

namespace Checkout.Domain.Tests.Transaction
{
    [TestFixture]
    internal abstract class TransactionTests
    {
        public class Create : TransactionTests
        {
            [TestCase("Paolo Regoli", "", "12", "2026", "123")]
            [TestCase("", "1234123412341234", "12", "2027", "123")]
            [TestCase("Paolo Regoli", "1234123412341234", "12", "2021", "123")]
            [TestCase("Paolo Regoli", "1234123412341234", "12", "2027", "")]
            public void Given_An_Invalid_CardDetails_Then_A_Domain_Exception_Is_Expected(
                string cardHolderName, string cardNumber, string expirationMonth, string expirationYear, string cvv)
            {
                //Arrange
                var cardDetails = new CardDetails
                {
                    CardHolderName = cardHolderName,
                    CardNumber = cardNumber,
                    ExpirationMonth = expirationMonth,
                    ExpirationYear = expirationYear,
                    Cvv = cvv
                };

                //Act & Assert
                Assert.Throws<InvalidCardException>(() => Domain.Transaction.Transaction.Create(Guid.NewGuid(), 100, cardDetails));
            }

            [TestCase("Paolo Regoli", "4242424242424242", "12", "2026", "100")]
            [TestCase("Paolo Regoli", "4543474002249996", "12", "2026", "956")]
            [TestCase("Paolo Regoli", "5436031030606378", "12", "2026", "257")]
            public void Given_Valid_Specifications_Then_A_Transaction_Should_Be_Created(
                string cardHolderName, string cardNumber, string expirationMonth, string expirationYear, string cvv)
            {
                //Arrange
                var merchantId = Guid.NewGuid();
                var cardDetails = new CardDetails
                {
                    CardHolderName = cardHolderName,
                    CardNumber = cardNumber,
                    ExpirationMonth = expirationMonth,
                    ExpirationYear = expirationYear,
                    Cvv = cvv
                };

                //Act
                var transaction = Domain.Transaction.Transaction.Create(merchantId, 100, cardDetails);

                //Assert
                Assert.AreEqual(merchantId, transaction.MerchantId);
                Assert.AreEqual(cardDetails, JsonSerializer.Deserialize<CardDetails>(transaction.CardDetails));
            }
        }
        
        public class Reject : TransactionTests
        {
            [Test]
            public void Given_A_Transaction_Rejection_Then_The_TransactionStatus_And_Description_Should_Match()
            {
                //Arrange
                var cardDetails = new CardDetails
                {
                    CardHolderName = "Paolo Regoli",
                    CardNumber = "4242424242424242",
                    ExpirationMonth = "12",
                    ExpirationYear = "2026",
                    Cvv = "100"
                };

                var transaction = Domain.Transaction.Transaction.Create(Guid.NewGuid(), 100, cardDetails);

                //Act
                transaction.Reject("error");

                //Assert
                Assert.AreEqual(TransactionStatus.Rejected, transaction.TransactionStatus);
                Assert.False(transaction.Successful);
            }
        }
        
        public class Authorize : TransactionTests
        {
            [Test]
            public void Given_A_Transaction_Authorization_Then_The_TransactionStatus_And_Description_Should_Match()
            {
                //Arrange
                var cardDetails = new CardDetails
                {
                    CardHolderName = "Paolo Regoli",
                    CardNumber = "4242424242424242",
                    ExpirationMonth = "12",
                    ExpirationYear = "2026",
                    Cvv = "100"
                };

                var transaction = Domain.Transaction.Transaction.Create(Guid.NewGuid(), 100, cardDetails);

                //Act
                transaction.Authorize();

                //Assert
                Assert.AreEqual(TransactionStatus.Authorized, transaction.TransactionStatus);
                Assert.True(transaction.Successful);
            }
        }
    }
}
