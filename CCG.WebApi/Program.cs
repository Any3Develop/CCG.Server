using CCG.Application.DI;
using CCG.Infrastructure.DI;
using CCG.WebApi.Infrastructure.Configurations;
using CCG.WebApi.Infrastructure.DI;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

builder.Services.InstallApplication();
builder.Services.InstallInfrastructure(builder.Configuration);
builder.Services.InstallWebApi(builder.Configuration);

var app = builder.Build();
await app.ConfigureWebApiAsync(app.Services);
await app.RunAsync();