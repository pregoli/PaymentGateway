using System.Text.RegularExpressions;
using Checkout.Domain.Transaction.Exceptions;
using Checkout.Domain.Transaction.ValueObjects;

namespace Checkout.Domain.Transaction.Specifications;

internal static class ValidCardDetailsSpecification
{
    internal static void Validate(CardDetails cardDetails)
    {
        if (cardDetails is null || string.IsNullOrEmpty(cardDetails.Number))
            throw new InvalidCardException("Missing card number");

        var monthCheck = new Regex(@"^(0[1-9]|1[0-2])$");
        var yearCheck = new Regex(@"^20[0-9]{2}$");
        var cvvCheck = new Regex(@"^\d{3}$");

        int sumOfDigits = cardDetails.Number.Where((e) => e >= '0' && e <= '9')
                .Reverse()
                .Select((e, i) => (e - 48) * (i % 2 == 0 ? 1 : 2))
                .Sum((e) => e / 10 + e % 10);

        var cardNumberisValid = sumOfDigits % 10 == 0 && Regex.IsMatch(cardDetails.Number, @"^\d+$");
        if (!cardNumberisValid)
            throw new InvalidCardException("Invalid card number provided.");
        if (cardDetails.Cvv == null || !cvvCheck.IsMatch(cardDetails.Cvv))
            throw new InvalidCardException("Invalid card cvv provided.");

        if (!monthCheck.IsMatch(cardDetails.ExpirationMonth) || !yearCheck.IsMatch(cardDetails.ExpirationYear))
            throw new InvalidCardException("Invalid card expiration date provided.");

        var year = int.Parse(cardDetails.ExpirationYear);
        var month = int.Parse(cardDetails.ExpirationMonth);
        var lastDateOfExpiryMonth = DateTime.DaysInMonth(year, month);
        var cardExpiry = new DateTime(year, month, lastDateOfExpiryMonth, 23, 59, 59);

        if (!(cardExpiry > DateTime.UtcNow))
            throw new InvalidCardException("Invalid card expiration date provided.");
    }
}
