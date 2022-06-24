using Checkout.Infrastructure.Persistence;
using Checkout.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using Checkout.Query.Application.Interfaces;
using Checkout.Command.Application.Interfaces;
using Checkout.Infrastructure.Providers;
using Checkout.Infrastructure.Instrumentation;

namespace Checkout.Infrastructure;

public static class Configuration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services
            .AddEntityFrameworkInMemoryDatabase()
            .AddDbContext<CheckoutDbContext>(opt => opt.UseInMemoryDatabase("Checkout"));

        services.AddScoped<IAcquiringBankProvider, AcquiringBankProvider>();

        services.AddScoped<ITransactionsHistoryCommandRepository, TransactionsHistoryCommandRepository>();
        services.AddScoped<ITransactionsHistoryQueryRepository, TransactionsHistoryQueryRepository>();

        services.AddHttpClient<IMetricsService, MetricsService>(
                    client =>
                    {
                        client.Timeout = TimeSpan.FromSeconds(180);
                    })
                .AddPolicyHandler(BaseRetryPolicy)
                .AddPolicyHandler(Policy.TimeoutAsync<HttpResponseMessage>(300));

        return services;
    }

    private static IAsyncPolicy<HttpResponseMessage> BaseRetryPolicy => HttpPolicyExtensions
          .HandleTransientHttpError()
          .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
}
