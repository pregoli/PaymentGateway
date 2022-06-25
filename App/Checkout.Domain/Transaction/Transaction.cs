using System.Text.Json;
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
            CardDetails cardDetails)
        {
            Id = id;
            MerchantId = merchantId;
            Amount = amount;
            CardDetails = JsonSerializer.Serialize(cardDetails);
            Status = TransactionStatus.Processing;
        }

        public Guid Id { get; private set; }
        public Guid MerchantId { get; private set; }
        public static string Currency => "GBP";
        public decimal Amount { get; private set; }
        public string CardDetails { get; private set; }
        public TransactionStatus Status { get; private set; }
        public string Description { get; private set; }
        public bool Successful => Status == TransactionStatus.Authorized;
        public DateTime Timestamp { get; private set; } = DateTime.UtcNow;

        public static Transaction Create(Guid merchantId, decimal amount, CardDetails cardDetails)
        {
            ValidMerchantIdSpecification.Validate(merchantId);
            ValidAmountSpecification.Validate(amount);
            ValidCardDetailsSpecification.Validate(cardDetails);

            return new Transaction(Guid.NewGuid(), merchantId, amount, cardDetails);
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