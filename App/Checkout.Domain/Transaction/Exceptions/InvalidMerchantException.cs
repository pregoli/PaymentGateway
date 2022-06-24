namespace Checkout.Domain.Transaction.Exceptions;

internal class InvalidMerchantException : Exception
{
    internal InvalidMerchantException(string message)
        : base(message)
    {
    }
}