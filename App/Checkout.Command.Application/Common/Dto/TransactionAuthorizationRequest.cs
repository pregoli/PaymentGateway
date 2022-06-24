using System;

namespace Checkout.Command.Application.Common.Dto;

public record TransactionAuthorizationRequest(Guid TransactionId, Guid MerchantId, decimal Amount);
