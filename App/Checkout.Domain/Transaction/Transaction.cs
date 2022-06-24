using System.Text.Json;
using Checkout.Domain.Transaction.Enums;
using Checkout.Domain.Transaction.Specifications;
using Checkout.Domain.Transaction.ValueObjects;

namespace Checkout.Domain.Transaction
{
    public class Transaction
    {
        private Transaction()
        {}

        private Transaction(
            Guid id,
            Guid merchantId,
            decimal amount,
            CardDetails cardDetails)
        {
            Id = id;
            MerchantId = merchantId;
            Amount = amount;
            CardDetails = JsonSerializer.Serialize(cardDetails);
            TransactionStatus = TransactionStatus.Processing;
        }

        public Guid Id { get; private set; }
        public Guid MerchantId { get; private set; }
        public static string Currency => "GBP";
        public decimal Amount { get; private set; }
        public string CardDetails { get; private set; }
        public TransactionStatus TransactionStatus { get; private set; }
        public string Description { get; private set; }
        public bool Successful => string.IsNullOrEmpty(Description);
        public DateTime Timestamp { get; private set; } = DateTime.UtcNow;

        public static Transaction Create(Guid merchantId, decimal amount, CardDetails cardDetails)
        {
            ValidCardDetailsSpecification.Validate(cardDetails);
            ValidMerchantIdSpecification.Validate(merchantId);

            return new Transaction(Guid.NewGuid(), merchantId, amount, cardDetails);
        }

        public void Reject(string description)
        {
            TransactionStatus = TransactionStatus.Rejected;
            Description = description;
        }
        
        public void Authorize()
        {
            TransactionStatus = TransactionStatus.Authorized;
        }
    }
}