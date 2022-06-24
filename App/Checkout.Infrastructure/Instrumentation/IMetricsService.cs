namespace Checkout.Infrastructure.Instrumentation;

public interface IMetricsService
{
    Task<string> GetAsync();
}