using System.Threading.Tasks;

namespace Checkout.Infrastructure.Instrumentation
{
    public interface IMetricsService
    {
        Task<string> Get();
    }
}