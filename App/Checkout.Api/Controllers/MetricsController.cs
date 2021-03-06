using Checkout.Infrastructure.Instrumentation;
using Microsoft.AspNetCore.Mvc;

namespace Checkout.Api.Controllers
{
    [ApiController]
    [Route("api")]
    [Consumes("application/json")]
    [Produces("application/json")]
    public class MetricsController : ControllerBase
    {
        private readonly IMetricsService _metricsService;

        public MetricsController(IMetricsService metricsService)
        {
            _metricsService = metricsService;
        }

        /// <summary>
        /// Retrieve the recorded application metrics
        /// </summary>
        /// <returns></returns>
        [HttpGet("beta/[controller]")]
        public async Task<ActionResult<string>> Get()
        {
            return await _metricsService.GetAsync();
        }
    }
}
