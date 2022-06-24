namespace Checkout.Command.Application.Dtos;

public record TransactionAuthorizationResponse(Guid TransactionId, bool Authorized, string Code, string Description);