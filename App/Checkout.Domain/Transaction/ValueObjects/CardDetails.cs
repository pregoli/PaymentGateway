namespace Checkout.Domain.Transaction.ValueObjects;

public record CardDetails
{
    public string CardHolderName { get; set; }

    public string CardNumber { get; set; }

    public string ExpirationMonth { get; set; }

    public string ExpirationYear { get; set; }

    public string Cvv { get; set; }
}
