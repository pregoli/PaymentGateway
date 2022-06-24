using Checkout.Domain.Transaction.Exceptions;
using Checkout.Domain.Transaction.Specifications;
using NUnit.Framework;

namespace Checkout.Domain.Tests.Transaction.Specifications;

[TestFixture]
public class ValidAmountSpecificationTests
{
    [TestCase(-1)]
    [TestCase(0)]
    public void Given_An_Invalid_Amount_Then_An_Exception_Is_Expected(decimal amount)
    {
        //Act & Assert
        Assert.Throws<InvalidAmountException>(() => ValidAmountSpecification.Validate(amount));
    }

    [TestCase(1)]
    [TestCase(2)]
    public void Given_A_Valid_Amount_Then_The_Specification_Is_Met(decimal amount)
    {
        //Act & Assert
        Assert.DoesNotThrow(() => ValidAmountSpecification.Validate(amount));
    }
}
