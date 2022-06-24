using Checkout.Command.Application.Common.Dto;

namespace Checkout.Command.Application.Interfaces
{
    public interface IAcquiringBankProvider
    {
        TransactionAuthorizationResponse ValidateTransaction(TransactionAuthorizationRequest payload);
    }
}
