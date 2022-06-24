using Checkout.Api;
using Checkout.Command.Application;
using Checkout.Infrastructure;
using Checkout.Query.Application;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddCommandApplication()
    .AddQueryApplication()
    .AddInfrastructure()
    .AddWebApiServices();

var app = builder.Build();

app.ConfigureWebApi();

app.Run();
