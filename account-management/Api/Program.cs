using PlatformPlatform.AccountManagement.Api;
using PlatformPlatform.AccountManagement.Api.Tenants;
using PlatformPlatform.AccountManagement.Api.Users;
using PlatformPlatform.AccountManagement.Application;
using PlatformPlatform.AccountManagement.Infrastructure;
using PlatformPlatform.SharedKernel.ApiCore;

var builder = WebApplication.CreateBuilder(args);

// Configure services for the Application, Infrastructure, and Api layers like Entity Framework, Repositories, MediatR,
// FluentValidation validators, Pipelines.
builder.Services
    .AddApplicationServices()
    .AddInfrastructureServices(builder.Configuration)
    .AddApiServices();

var app = builder.Build();

// Add configuration common for all web applications like Swagger, HSTS, and UseDeveloperExceptionPage.
app.AddCommonConfiguration();

app.MapTenantEndpoints();
app.MapUserEndpoints();

app.Run();