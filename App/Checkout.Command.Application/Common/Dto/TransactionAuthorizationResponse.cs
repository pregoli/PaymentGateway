using System;

namespace Checkout.Command.Application.Common.Dto;

public record TransactionAuthorizationResponse(Guid TransactionId, bool Authorized, string Code, string Description);