using Checkout.Api.Requests;
using Checkout.Command.Application.Interfaces;
using Checkout.Query.Application.Dtos;
using Checkout.Query.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;

namespace Checkout.Api.Controllers;

[Route("api")]
[ApiController]
[Produces("application/json")]
public class TransactionsController : ControllerBase
{
    private readonly ICheckoutCommandApplication _checkoutCommandApplication;
    private readonly ICheckoutQueryApplication _checkoutQueryApplication;

    public TransactionsController(ICheckoutCommandApplication checkoutCommandApplication, ICheckoutQueryApplication checkoutQueryApplication)
    {
        _checkoutCommandApplication = checkoutCommandApplication;
        _checkoutQueryApplication = checkoutQueryApplication;
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
    public async Task<IActionResult> ExecutePayment([FromBody] TransactionRequest request)
    {
        var transactionId = await _checkoutCommandApplication.ExecutePayment(request.MerchantId, request.CardDetails, request.Amount);

        return CreatedAtRoute(nameof(GetTransactionById), new { transactionId }, new { transactionId });
    }

    /// <summary>
    /// Retrieve a specific transaction by its unique id
    /// </summary>
    /// <param name="transactionId">The transaction unique id</param>
    /// <returns></returns>
    [HttpGet("beta/[controller]/{transactionId}", Name = nameof(GetTransactionById))]
    [ProducesResponseType(200, Type = typeof(TransactionResponse))]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetTransactionById([FromRoute] Guid transactionId)
    {
        var transaction = await _checkoutQueryApplication.GetTransactionByIdAsync(transactionId);
        return transaction is not null ? Ok(transaction) : NotFound();
    }

    /// <summary>
    /// Retrieve a collection of transactions by merchant id
    /// </summary>
    /// <param name="merchantId">The merchant id</param>
    /// <returns></returns>
    [HttpGet("beta/Merchants/{merchantId}/[controller]", Name = nameof(GetTransactionByMerchantId))]
    [ProducesResponseType(200, Type = typeof(List<TransactionResponse>))]
    public async Task<ActionResult<List<TransactionResponse>>> GetTransactionByMerchantId([FromRoute] Guid merchantId)
    {
        var transactions = await _checkoutQueryApplication.GetTransactionsByMerchantIdAsync(merchantId);
        return Ok(transactions);
    }
}
