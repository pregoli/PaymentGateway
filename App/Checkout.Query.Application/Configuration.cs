using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Checkout.Query.Application.Interfaces;

namespace Checkout.Query.Application;

public static class Configuration
{
    public static IServiceCollection AddQueryApplication(this IServiceCollection services)
    {
        services.AddMediatR(Assembly.GetExecutingAssembly());
        services.AddTransient<ICheckoutQueryApplication, CheckoutQueryApplication>();

        return services;
    }
}