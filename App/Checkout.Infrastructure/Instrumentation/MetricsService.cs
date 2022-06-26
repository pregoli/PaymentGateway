using Microsoft.Extensions.Logging;

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

        public async Task<string?> GetAsync()
        {
            var response = await _httpClient.GetAsync($"metrics");
            return response.IsSuccessStatusCode ? await response.Content.ReadAsStringAsync() : response.ReasonPhrase;
        }
    }
}
