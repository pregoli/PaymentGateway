using System.ComponentModel.DataAnnotations.Schema;
using Checkout.Domain.Transaction.Enums;
using Checkout.Domain.Transaction.Specifications;
using Checkout.Domain.Transaction.ValueObjects;

namespace Checkout.Domain.Transaction
{
    public class Transaction
    {
        private Transaction() {}

        private Transaction(
            Guid id,
            Guid merchantId,
            decimal amount,
            CardDetails creditCard)
        {
            Id = id;
            MerchantId = merchantId;
            Amount = amount;
            CardHolderName = creditCard.HolderName;
            CardNumber = creditCard.Number;
            CardExpirationMonth = creditCard.ExpirationMonth;
            CardExpirationYear = creditCard.ExpirationYear;
            CardCvv = creditCard.Cvv;
            Status = TransactionStatus.Processing;
        }

        public Guid Id { get; private set; }
        public Guid MerchantId { get; private set; }
        public static string Currency => "GBP";
        public decimal Amount { get; private set; }
        [NotMapped]
        public CardDetails CreditCard { get; private set; }
        public string CardHolderName { get; private set; }
        public string CardNumber { get; private set; }
        public string CardExpirationMonth { get; private set; }
        public string CardExpirationYear { get; private set; }
        public string CardCvv { get; }
        public TransactionStatus Status { get; private set; }
        public string Description { get; private set; }
        public bool Successful => Status == TransactionStatus.Authorized;
        public DateTime Timestamp { get; private set; } = DateTime.UtcNow;

        public static Transaction Create(Guid merchantId, decimal amount, CardDetails creditCard)
        {
            ValidMerchantIdSpecification.Validate(merchantId);
            ValidAmountSpecification.Validate(amount);
            ValidCardDetailsSpecification.Validate(creditCard);

            return new Transaction(Guid.NewGuid(), merchantId, amount, creditCard);
        }

        public void Reject(string description)
        {
            Status = TransactionStatus.Rejected;
            Description = description;
        }
        
        public void Authorize()
        {
            Status = TransactionStatus.Authorized;
        }
    }
}