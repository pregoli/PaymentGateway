using Checkout.Domain.Transaction.Exceptions;

namespace Checkout.Domain.Transaction.Specifications;

internal class ValidAmountSpecification
{
    internal static void Validate(decimal amount)
    {
        if (amount <= 0)
            throw new InvalidAmountException($"The provided amount is invalid: {amount}");
    }
}