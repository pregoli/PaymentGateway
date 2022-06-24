using Checkout.Domain.Transaction.Exceptions;
using Checkout.Domain.Transaction.Specifications;
using NUnit.Framework;

namespace Checkout.Domain.Tests.Transaction.Specifications;

[TestFixture]
internal class ValidMerchantIdSpecificationTests
{
    [Test]
    public void Given_An_Invalid_MerchantId_Then_An_Exception_Is_Expected()
    {
        //Act & Assert
        Assert.Throws<InvalidMerchantException>(() => ValidMerchantIdSpecification.Validate(default));
    }

    [Test]
    public void Given_A_Valid_MerchantId_Then_The_Specification_Is_Met()
    {
        //Act & Assert
        Assert.DoesNotThrow(() => ValidMerchantIdSpecification.Validate(Guid.NewGuid()));
    }
}
