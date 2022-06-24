using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Threading.Tasks;

namespace Checkout.Infrastructure.Instrumentation
{
    public class MetricsService : IMetricsService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<MetricsService> _logger;

        public MetricsService(HttpClient httpClient, ILogger<MetricsService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<string> Get()
        {
            var response = await _httpClient.GetAsync($"metrics");
            return response.IsSuccessStatusCode ? await response.Content.ReadAsStringAsync() : response.ReasonPhrase;
        }
    }
}
