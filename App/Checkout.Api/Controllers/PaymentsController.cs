using Checkout.Api.Requests;
using Checkout.Command.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;

namespace Checkout.Api.Controllers;

[ApiController]
[Route("api")]
[Consumes("application/json")]
[Produces("application/json")]
public class PaymentsController : ControllerBase
{
    private readonly ICheckoutCommandApplication _checkoutCommandApplication;

    public PaymentsController(ICheckoutCommandApplication checkoutCommandApplication)
    {
        _checkoutCommandApplication = checkoutCommandApplication;
    }

    /// <summary>
    /// Submit a payment
    /// </summary>
    /// <param name="request">It Includes all required informations to submit a payment</param>
    /// <returns></returns>
    [HttpPost("beta/[controller]")]
    [SwaggerRequestExample(typeof(TransactionRequest), typeof(TransactionRequestExample))]
    [ProducesResponseType(201, Type = typeof(Guid))]
    [ProducesResponseType(400)]
    public async Task<IActionResult> SubmitPayment([FromBody] TransactionRequest request)
    {
        var transactionId = await _checkoutCommandApplication.SubmitPayment(request.MerchantId, request.CardDetails, request.Amount);

        return CreatedAtRoute("GetTransactionById", new { transactionId }, new { transactionId });
    }
}