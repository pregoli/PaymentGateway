using System.Net;
using System.Text.Json;
using Checkout.Domain.Transaction.Enums;
using Checkout.Domain.Transaction.ValueObjects;
using Checkout.Query.Application.Extensions;

namespace Checkout.Query.Application.Dtos;

public record TransactionResponse
{
    private TransactionResponse(
        Guid transactionId,
        Guid merchantId,
        string cardHolderName,
        string cardNumber,
        decimal amount,
        string status,
        string description,
        DateTime timestamp)
    {
        TransactionId = transactionId;
        MerchantId = merchantId;
        CardHolderName = cardHolderName;
        CardNumber = cardNumber;
        Amount = amount;
        Status = status;
        Description = description;
        Timestamp = timestamp;
        Currency = "GBP";
    }

    public Guid TransactionId { get; init; }
    public Guid MerchantId { get; init; }
    public string CardHolderName { get; init; }
    public string CardNumber { get; init; }
    public decimal Amount { get; init; }
    public string Status { get; init; }
    public string Description { get; init; }
    public DateTime Timestamp { get; init; }
    public string Currency { get; init; }
    public bool Successful => Status == TransactionStatus.Authorized.ToString();
    
    public static TransactionResponse Map(
        Guid transactionId,
        Guid merchantId,
        string stringfiedCardDetails,
        decimal amount,
        string status,
        string description,
        DateTime timestamp)
    {
        var cardDetails = JsonSerializer.Deserialize<CardDetails>(stringfiedCardDetails);
        return new TransactionResponse(
            transactionId,
            merchantId,
            cardDetails.CardHolderName,
            cardDetails.CardNumber.Mask('X'),
            amount,
            status,
            description,
            timestamp);
    }
}