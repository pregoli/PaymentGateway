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
        internal class Create : TransactionTests
        {
            [TestCaseSource(nameof(InvalidCardDetailsCaseSource))]
            public void Given_An_Invalid_CardDetails_Then_A_Domain_Exception_Is_Expected(CardDetails cardDetails)
            {
                //Act & Assert
                Assert.Throws<InvalidCardException>(() => Domain.Transaction.Transaction.Create(Guid.NewGuid(), 100, cardDetails));
            }

            [TestCaseSource(nameof(ValidCardDetailsCaseSource))]
            public void Given_Valid_Specifications_Then_A_Transaction_Should_Be_Created(CardDetails cardDetails)
            {
                //Arrange
                var merchantId = Guid.NewGuid();

                //Act
                var transaction = Domain.Transaction.Transaction.Create(merchantId, 100, cardDetails);

                //Assert
                Assert.AreEqual(merchantId, transaction.MerchantId);
                Assert.AreEqual(cardDetails, JsonSerializer.Deserialize<CardDetails>(transaction.CardDetails));
            }
        }

        internal class Reject : TransactionTests
        {
            [TestCaseSource(nameof(ValidCardDetailsCaseSource))]
            public void Given_A_Transaction_Rejection_Then_The_Status_Should_Match(CardDetails cardDetails)
            {
                //Arrange
                var transaction = Domain.Transaction.Transaction.Create(Guid.NewGuid(), 100, cardDetails);

                //Act
                transaction.Reject("error");

                //Assert
                Assert.AreEqual(TransactionStatus.Rejected, transaction.Status);
                Assert.False(transaction.Successful);
            }
        }

        internal class Authorize : TransactionTests
        {
            [TestCaseSource(nameof(ValidCardDetailsCaseSource))]
            public void Given_A_Transaction_Authorization_Then_The_Status_Should_Match(CardDetails cardDetails)
            {
                //Arrange
                var transaction = Domain.Transaction.Transaction.Create(Guid.NewGuid(), 100, cardDetails);

                //Act
                transaction.Authorize();

                //Assert
                Assert.AreEqual(TransactionStatus.Authorized, transaction.Status);
                Assert.True(transaction.Successful);
            }
        }

        protected static readonly object[] InvalidCardDetailsCaseSource = {
                new object[]
                {
                    new CardDetails
                    {
                        CardHolderName = "Paolo Regoli",
                        CardNumber = "",
                        Cvv = "123",
                        ExpirationMonth = "12",
                        ExpirationYear = "2026"
                    }
                },
                new object[]
                {
                    new CardDetails
                    {
                        CardHolderName = "Paolo Regoli",
                        CardNumber = "1234123412341234",
                        Cvv = "956",
                        ExpirationMonth = "12",
                        ExpirationYear = "2021"
                    }
                },
                new object[]
                {
                    new CardDetails
                    {
                        CardHolderName = "Paolo Regoli",
                        CardNumber = "5436031030606378",
                        Cvv = "",
                        ExpirationMonth = "12",
                        ExpirationYear = "2026"
                    }
                }
            };
        
        protected static readonly object[] ValidCardDetailsCaseSource = {
                new object[]
                {
                    new CardDetails
                    {
                        CardHolderName = "Paolo Regoli",
                        CardNumber = "4242424242424242",
                        Cvv = "100",
                        ExpirationMonth = "12",
                        ExpirationYear = "2026"
                    }
                },
                new object[]
                {
                    new CardDetails
                    {
                        CardHolderName = "Paolo Regoli",
                        CardNumber = "4543474002249996",
                        Cvv = "956",
                        ExpirationMonth = "12",
                        ExpirationYear = "2026"
                    }
                },
                new object[]
                {
                    new CardDetails
                    {
                        CardHolderName = "Paolo Regoli",
                        CardNumber = "5436031030606378",
                        Cvv = "257",
                        ExpirationMonth = "12",
                        ExpirationYear = "2026"
                    }
                }
            };
    }
}
