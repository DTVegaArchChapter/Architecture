using FluentValidation;
using Mediator;
using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Application.Common.Behaviors;
using RestaurantManagement.Domain.Repositories;
using RestaurantManagement.Infrastructure.Data;
using RestaurantManagement.Infrastructure.Repositories;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

// Database context - using In-Memory for demo
builder.Services.AddDbContext<RestaurantDbContext>(options =>
    options.UseInMemoryDatabase("RestaurantDb"));

// Repository registration (Infrastructure layer)
builder.Services.AddScoped<ITableRepository, TableRepository>();
builder.Services.AddScoped<IMenuItemRepository, MenuItemRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Register FluentValidation validators from Application assembly
builder.Services.AddValidatorsFromAssembly(
    typeof(RestaurantManagement.Application.Orders.Commands.CreateOrder.CreateOrderCommandHandler).Assembly, 
    ServiceLifetime.Singleton);

// Mediator - source generator based, high performance with pipeline behaviors
builder.Services.AddMediator(options =>
{
    options.ServiceLifetime = ServiceLifetime.Scoped;
});

// Register ValidationBehavior for pipeline
builder.Services.AddSingleton(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

var app = builder.Build();

// Seed database
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<RestaurantDbContext>();
    await context.Database.EnsureCreatedAsync().ConfigureAwait(false);
}

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

await app.RunAsync().ConfigureAwait(false);

// Make Program class accessible for testing
public partial class Program { }