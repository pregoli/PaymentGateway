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
            public void Given_Valid_Specifications_Then_A_Transaction_Should_Be_Created(CardDetails creditCard)
            {
                //Arrange
                var merchantId = Guid.NewGuid();

                //Act
                var transaction = Domain.Transaction.Transaction.Create(merchantId, 100, creditCard);

                //Assert
                transaction.MerchantId.Should().Be(merchantId);
                transaction.CardHolderName.Should().Be(creditCard.HolderName);
                transaction.CardNumber.Should().Be(creditCard.Number);
                transaction.CardExpirationMonth.Should().Be(creditCard.ExpirationMonth);
                transaction.CardExpirationYear.Should().Be(creditCard.ExpirationYear);
                transaction.CardCvv.Should().Be(creditCard.Cvv);
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
                    CardDetails.Create("Paolo Regoli", "", "12", "2026", "123")
                },
                new object[]
                {
                    CardDetails.Create("Paolo Regoli", "1234123412341234", "12", "2021", "956")
                },
                new object[]
                {
                    CardDetails.Create("Paolo Regoli", "5436031030606378", "12", "2026", "")
                }
            };
        
        protected static readonly object[] ValidCardDetailsCaseSource = {
                new object[]
                {
                    CardDetails.Create("Paolo Regoli", "4242424242424242", "12", "2026", "100")
                },
                new object[]
                {
                    CardDetails.Create("Paolo Regoli", "4543474002249996", "12", "2026", "956")
                },
                new object[]
                {
                    CardDetails.Create("Paolo Regoli", "5436031030606378", "12", "2026", "257")
                }
            };
    }
}
