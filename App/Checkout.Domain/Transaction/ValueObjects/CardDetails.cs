namespace Checkout.Domain.Transaction.ValueObjects;

public record CardDetails
{
    private CardDetails(string cardHolderName, string cardNumber, string expirationMonth, string expirationYear, string cvv)
    {
        HolderName = cardHolderName;
        Number = cardNumber;
        ExpirationMonth = expirationMonth;
        ExpirationYear = expirationYear;
        Cvv = cvv;
    }

    public string HolderName { get; }

    public string Number { get; }

    public string ExpirationMonth { get; }

    public string ExpirationYear { get; }

    public string Cvv { get; }

    public static CardDetails Create(string cardHolderName, string cardNumber, string expirationMonth, string expirationYear, string cvv)
    {
        return new CardDetails(cardHolderName, cardNumber, expirationMonth, expirationYear, cvv);
    }
}
