using System;

namespace Checkout.Domain.Transaction.Exceptions;

public class InvalidCardException : Exception
{
    public InvalidCardException(string message)
        : base(message)
    {
    }
}
