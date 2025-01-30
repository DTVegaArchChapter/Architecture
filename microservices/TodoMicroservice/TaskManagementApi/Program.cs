using MessageQueue;

using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

using TaskManagementApi.Events;
using TaskManagementApi.HostedServices;
using TaskManagementApi.Hubs;
using TaskManagementApi.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHostedService<GeneralNotificationHandler>();
builder.Services.AddSingleton<IRabbitMqConnection>(_ => new DefaultRabbitMqConnection(builder.Configuration.GetSection("RabbitMqConnection").Get<RabbitMqConnectionSettings>()!));
builder.Services.AddSingleton<IMessageQueueConsumerService<GeneralNotificationEvent>>(x => new RabbitMqMessageQueueConsumerService<GeneralNotificationEvent>(x.GetRequiredService<IRabbitMqConnection>(), builder.Configuration.GetSection("RabbitMqConsumerGeneralNotification").Get<RabbitMqConsumerSettings>()!));
builder.Services.AddSingleton<IMessageQueuePublisherService, RabbitMqMessageQueuePublisherService>();

builder.Services.AddControllers();
builder.Services.AddSignalR();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<TaskDbContext>(options =>
    options.UseNpgsql("Host=postgres;Database=taskdb;Username=postgres;Password=postgres"));

builder.Services.AddCors(options =>
    {
        options.AddPolicy(name: "CorsOrigins",
            policy =>
                {
                    policy
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .SetIsOriginAllowed(_ => true)
                        .AllowCredentials();
                });
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(config =>
        {
            config.PreSerializeFilters.Add((document, request) =>
                {
                    var externalPath = !request.Headers.ContainsKey("x-envoy-original-path") ? string.Empty :
                                           request.Headers["x-envoy-original-path"][0]!.Replace("swagger/v1/swagger.json", string.Empty);
                    if (!string.IsNullOrWhiteSpace(externalPath))
                    {
                        var newPaths = new OpenApiPaths();
                        foreach (var path in document.Paths)
                        {
                            newPaths[$"{externalPath.TrimEnd('/')}{path.Key}"] = path.Value;
                        }

                        document.Paths = newPaths;
                    }
                });
        });
    app.UseSwaggerUI();
}

app.UseCors("CorsOrigins");
app.UseAuthorization();

app.MapControllers();
app.MapHub<GeneralNotificationHub>("/generalNotificationHub");
await using (var serviceScope = app.Services
                 .GetRequiredService<IServiceScopeFactory>()
                 .CreateAsyncScope())
{
    await using (var context = serviceScope.ServiceProvider.GetRequiredService<TaskDbContext>())
    {
        await context.Database.MigrateAsync().ConfigureAwait(false);
    }
}

await app.RunAsync().ConfigureAwait(false);
