using System;

namespace Checkout.Command.Application.Common.Dto;

public class TransactionHistoryDto
{
    public TransactionHistoryDto()
    {
    }

    public TransactionHistoryDto(
        Guid id,
        Guid merchantId,
        decimal amount,
        string cardHolderName,
        string cardNumber,
        string statusCode,
        string description)
    {
        Id = id;
        MerchantId = merchantId;
        Amount = amount;
        CardHolderName = cardHolderName;
        CardNumber = cardNumber;
        StatusCode = statusCode;
        Description = description;
        Timestamp = DateTime.UtcNow;
    }

    public Guid Id { get; private set; }
    public Guid MerchantId { get; private set; }
    public static string Currency => "GBP";
    public decimal Amount { get; private set; }
    public string CardHolderName { get; private set; }
    public string CardNumber { get; private set; }
    public string StatusCode { get; private set; }
    public string Description { get; private set; }
    public DateTime Timestamp { get; private set; }
}
