using Checkout.Domain.Transaction.ValueObjects;
using System;
using System.ComponentModel.DataAnnotations;

namespace Checkout.Api.Requests;

public class TransactionRequest
{
    [Required]
    public Guid MerchantId { get; set; }

    [Required]
    public CardDetails CardDetails { get; set; }

    [Required]
    [Range(1, double.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
    public decimal Amount { get; set; }
}
