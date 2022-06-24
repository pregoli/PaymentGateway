using Checkout.Command.Application.Dtos;

namespace Checkout.Command.Application.Interfaces
{
    public interface IAcquiringBankProvider
    {
        TransactionAuthorizationResponse ValidateTransaction(TransactionAuthorizationRequest payload);
    }
}
