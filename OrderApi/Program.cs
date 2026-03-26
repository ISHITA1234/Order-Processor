using Microsoft.Azure.Cosmos;
using Order.Infrastructure.Services;
using Order.Application.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();

builder.Services.AddSingleton<CosmosClient>(
    new CosmosClient(builder.Configuration["CosmosDb:ConnectionString"])
);

builder.Services.AddScoped<CosmosDbService>();
builder.Services.AddScoped<IMessagePublisher, ServiceBusPublisher>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();