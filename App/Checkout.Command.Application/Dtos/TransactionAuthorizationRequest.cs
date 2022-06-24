namespace Checkout.Command.Application.Dtos;

public record TransactionAuthorizationRequest(Guid TransactionId, Guid MerchantId, decimal Amount);
