namespace Checkout.Domain.Transaction.Exceptions;

public class InvalidMerchantException : DomainException
{
    public InvalidMerchantException(string message)
        : base(message)
    {
    }
}