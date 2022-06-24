using System.Text.Json.Serialization;
using Checkout.Api.Requests;
using Checkout.Infrastructure.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Prometheus;
using Swashbuckle.AspNetCore.Filters;

namespace Checkout.Api;

internal static class Configuration
{
    internal static IServiceCollection AddWebApiServices(this IServiceCollection services)
    {
        services
            .AddRouting(options => options.LowercaseUrls = true)
            .AddControllers()
            .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

        services.AddEndpointsApiExplorer();

        services.AddSwaggerExamplesFromAssemblyOf<TransactionRequestExample>();
        services.AddSwaggerGen(options =>
        {
            options.SupportNonNullableReferenceTypes();
            options.ExampleFilters();
            options.UseInlineDefinitionsForEnums();
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "Checkout Gateway API", Version = "Beta" });
        });

        services.AddResponseCompression();

        return services;
    }

    internal static WebApplication ConfigureWebApi(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseMetricServer("/api/beta/metrics");
        app.UseHttpMetrics();
        app.UseHttpsRedirection();
        app.MapControllers();

        MigrateDatabase(app);

        return app;
    }

    private static void MigrateDatabase(IApplicationBuilder builder)
    {
        using (var serviceScope = builder.ApplicationServices
            .GetRequiredService<IServiceScopeFactory>()
            .CreateScope())
        {
            using (var context = serviceScope.ServiceProvider.GetService<CheckoutDbContext>())
            {
                context.Database.EnsureCreated();
            }
        }
    }
}
