using FluentValidation;
using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Checkout.Command.Application.Interfaces;

namespace Checkout.Command.Application;

public static class Configuration
{
    public static IServiceCollection AddCommandApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddMediatR(Assembly.GetExecutingAssembly());

        services.AddTransient<ICheckoutCommandApplication, CheckoutCommandApplication>();

        return services;
    }
}