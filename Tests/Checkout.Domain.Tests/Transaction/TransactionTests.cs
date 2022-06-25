using System.Text.Json;
using Checkout.Domain.Transaction.Enums;
using Checkout.Domain.Transaction.Exceptions;
using Checkout.Domain.Transaction.ValueObjects;
using FluentAssertions;
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
                //Act
                Action acttion = () => Domain.Transaction.Transaction.Create(Guid.NewGuid(), 100, cardDetails);

                //Assert
                acttion.Should().Throw<InvalidCardException>();
            }

            [TestCaseSource(nameof(ValidCardDetailsCaseSource))]
            public void Given_Valid_Specifications_Then_A_Transaction_Should_Be_Created(CardDetails cardDetails)
            {
                //Arrange
                var merchantId = Guid.NewGuid();

                //Act
                var transaction = Domain.Transaction.Transaction.Create(merchantId, 100, cardDetails);

                //Assert
                transaction.MerchantId.Should().Be(merchantId);
                JsonSerializer.Deserialize<CardDetails>(transaction.CardDetails).Should().Be(cardDetails);
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
                transaction.Status.Should().Be(TransactionStatus.Rejected);
                transaction.Successful.Should().BeFalse();
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
                transaction.Status.Should().Be(TransactionStatus.Authorized);
                transaction.Successful.Should().BeTrue();
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
