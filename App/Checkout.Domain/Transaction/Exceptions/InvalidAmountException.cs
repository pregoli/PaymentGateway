namespace Checkout.Domain.Transaction.Exceptions;

internal class InvalidAmountException : Exception
{
    internal InvalidAmountException(string message)
        : base(message)
    {
    }
}