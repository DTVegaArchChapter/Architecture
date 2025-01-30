using AuditLogWorkerService;
using AuditLogWorkerService.Infrastructure.Data;

using MessageQueue;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddSingleton<IRabbitMqConnection>(_ => new DefaultRabbitMqConnection(context.Configuration.GetSection("RabbitMqConnection").Get<RabbitMqConnectionSettings>()!));
        services.AddSingleton(context.Configuration.GetSection("RabbitMqConsumer").Get<RabbitMqConsumerSettings>()!);
        services.AddSingleton<IMessageQueueConsumerService, RabbitMqGenericMessageQueueConsumerService>();
        services.AddSingleton<IMessageQueuePublisherService, RabbitMqMessageQueuePublisherService>();

        services.AddSingleton(context.Configuration.GetSection("MongoDb").Get<MongoDbSettings>()!);
        services.AddSingleton<IRepository, MongoDbRepository>();

        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
