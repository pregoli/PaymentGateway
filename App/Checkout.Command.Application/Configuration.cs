using FluentValidation;
using System.Reflection;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Checkout.Command.Application.Interfaces;
using Checkout.Command.Application.Common.Mappings;

namespace Checkout.Command.Application
{
    public static class Configuration
    {
        public static IServiceCollection AddCommandApplication(this IServiceCollection services)
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            services.AddSingleton(mappingConfig.CreateMapper());

            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());

            services.AddTransient<ICheckoutCommandApplication, CheckoutCommandApplication>();

            return services;
        }
    }
}
