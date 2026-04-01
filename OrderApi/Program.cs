using Microsoft.Azure.Cosmos;
using Order.Infrastructure.Services;
using Order.Application.Interfaces;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Azure.Identity;

var builder = WebApplication.CreateBuilder(args);

// Adding Azure key vault
// var keyVaultUrl = new Uri("https://order-keyvault.vault.azure.net/");

// builder.Configuration.AddAzureKeyVault(
//     keyVaultUrl,
//     new DefaultAzureCredential()
// );

// Add services
builder.Services.AddControllers();

// Azure CosmosDB connection string
builder.Services.AddSingleton<CosmosClient>(
    new CosmosClient(builder.Configuration["CosmosDb"])
);

builder.Services.AddScoped<CosmosDbService>();
builder.Services.AddScoped<IMessagePublisher, ServiceBusPublisher>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Application Insight
builder.Services.AddApplicationInsightsTelemetry();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.MapGet("/", () => "API is running 🚀");

app.Run();