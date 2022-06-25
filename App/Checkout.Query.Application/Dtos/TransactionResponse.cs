using System.Text.Json;
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
        string transactionStatus,
        string description,
        DateTime timestamp)
    {
        TransactionId = transactionId;
        MerchantId = merchantId;
        CardHolderName = cardHolderName;
        CardNumber = cardNumber;
        Amount = amount;
        TransactionStatus = transactionStatus;
        Description = description;
        Timestamp = timestamp;
        Currency = "GBP";
    }

    public Guid TransactionId { get; init; }
    public Guid MerchantId { get; init; }
    public string CardHolderName { get; init; }
    public string CardNumber { get; init; }
    public decimal Amount { get; init; }
    public string TransactionStatus { get; init; }
    public string Description { get; init; }
    public DateTime Timestamp { get; init; }
    public string Currency { get; init; }
    public bool Successful => string.IsNullOrEmpty(Description);

    public static TransactionResponse Unprocessable(
        Guid transactionId, string transactionStatus, string description)
    {
        return new TransactionResponse(
            transactionId,
            merchantId: default,
            cardHolderName: default,
            cardNumber: default,
            amount: default,
            transactionStatus,
            description,
            timestamp: default);
    }
    
    public static TransactionResponse Map(
        Guid transactionId,
        Guid merchantId,
        string stringfiedCardDetails,
        decimal amount,
        string transactionStatus,
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
            transactionStatus,
            description,
            timestamp);
    }
}