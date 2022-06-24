namespace Checkout.Domain.Transaction.Exceptions;

internal class InvalidCardException : Exception
{
    internal InvalidCardException(string message)
        : base(message)
    {
    }
}
