using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using SmartReturns.Consumers;
using SmartReturns.Models;
using SmartReturns.Services;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Database (PostgreSQL)
builder.Services.AddDbContext<ReturnsDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres")));

// Database (MSSQL)
builder.Services.AddDbContext<ProductDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MSSQL")));

// Redis
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
    ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("Redis")!));

// Services
builder.Services.AddScoped<IReturnService, ReturnService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddSingleton<INotificationHandler, NotificationHandler>();
builder.Services.AddSingleton<ReturnCreatedConsumer>();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

var consumer = app.Services.GetRequiredService<ReturnCreatedConsumer>();
consumer.StartListening();

app.MapGet("/returns/{id}", async (int id, IReturnService service) =>
    await service.GetReturnAsync(id));

app.MapPost("/returns", async (ReturnRequest request, IReturnService service) =>
    await service.CreateReturnAsync(request));

app.MapGet("/products/{id}", async (int id, IProductService service) =>
    await service.GetProductByIdAsync(id));

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
