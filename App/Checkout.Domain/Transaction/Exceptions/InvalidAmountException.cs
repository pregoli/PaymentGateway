﻿namespace Checkout.Domain.Transaction.Exceptions;

public class InvalidAmountException : DomainException
{
    public InvalidAmountException(string message)
        : base(message)
    {
    }
}