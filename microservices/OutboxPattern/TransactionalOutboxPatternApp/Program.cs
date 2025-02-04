using MessageQueue;
using Microsoft.EntityFrameworkCore;
using TransactionalOutboxPatternApp.Infrastructure.Data;
using TransactionalOutboxPatternApp.Infrastructure.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddDbContext<OrderDbContext>(options => options.UseSqlServer("name=ConnectionStrings:DefaultConnection"));
builder.Services.AddSingleton<IRabbitMqConnection>(_ => new DefaultRabbitMqConnection(builder.Configuration.GetSection("RabbitMqConnection").Get<RabbitMqConnectionSettings>()));
builder.Services.AddSingleton<IMessageQueuePublisherService, RabbitMqMessageQueuePublisherService>();

builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IEventLogService, EventLogService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

using (var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
{
    serviceScope.ServiceProvider.GetRequiredService<OrderDbContext>().Database.EnsureCreated();
}

app.Run();
