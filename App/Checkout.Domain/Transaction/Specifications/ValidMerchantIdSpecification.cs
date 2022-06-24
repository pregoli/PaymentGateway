using Checkout.Domain.Transaction.Exceptions;

namespace Checkout.Domain.Transaction.Specifications;

internal class ValidMerchantIdSpecification
{
    internal static void Validate(Guid merchantId)
    {
        if (merchantId == Guid.Empty)
            throw new InvalidMerchantException($"The provided merchant id is invalid: {merchantId}");
    }
}