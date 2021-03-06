using Checkout.Query.Application.Dtos;
using Checkout.Query.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Checkout.Api.Controllers;

[ApiController]
[Route("api")]
[Consumes("application/json")]
[Produces("application/json")]
public class TransactionsController : ControllerBase
{
    private readonly ICheckoutQueryApplication _checkoutQueryApplication;

    public TransactionsController(ICheckoutQueryApplication checkoutQueryApplication)
    {
        _checkoutQueryApplication = checkoutQueryApplication;
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
