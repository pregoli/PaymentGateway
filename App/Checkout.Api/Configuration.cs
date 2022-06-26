using System.Text.Json.Serialization;
using Checkout.Api.Requests;
using Checkout.Domain.Transaction.Exceptions;
using Checkout.Infrastructure.Persistence;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Prometheus;
using Swashbuckle.AspNetCore.Filters;

namespace Checkout.Api;

internal static class Configuration
{
    internal static IServiceCollection AddWebApiServices(this IServiceCollection services)
    {
        services.AddProblemDetails(setup =>
        {
            setup.IncludeExceptionDetails = (ctx, env) => false;
            setup.Map<DomainException>(exception => new ProblemDetails
            {
                Detail = exception.Message,
                Status = StatusCodes.Status400BadRequest,
                Type = exception.GetType().ToString()
            });
        });

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
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "Payment Gateway API", Version = "Beta" });
        });

        services.AddResponseCompression();

        return services;
    }

    internal static WebApplication ConfigureWebApi(this WebApplication app)
    {
        app.UseProblemDetails();
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
        using (var serviceScope = builder.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
        {
            using (var context = serviceScope.ServiceProvider.GetService<CheckoutDbContext>())
            {
                context!.Database.EnsureCreated();
            }
        }
    }
}
