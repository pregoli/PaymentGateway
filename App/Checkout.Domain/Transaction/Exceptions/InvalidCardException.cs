namespace Checkout.Domain.Transaction.Exceptions;

public class InvalidCardException : DomainException
{
    public InvalidCardException(string message)
        : base(message)
    {
    }
}
