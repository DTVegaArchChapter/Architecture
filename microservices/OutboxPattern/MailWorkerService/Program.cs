using MailWorkerService;
using MailWorkerService.Infrastructure.Events;

using MessageQueue;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddHostedService<Worker>();

        services.AddSingleton<IRabbitMqConnection>(_ => new DefaultRabbitMqConnection(context.Configuration.GetSection("RabbitMqConnection").Get<RabbitMqConnectionSettings>()!));
        services.AddSingleton(context.Configuration.GetSection("RabbitMqConsumer").Get<RabbitMqConsumerSettings>()!);
        services.AddSingleton<IMessageQueueConsumerService<OrderCreatedEvent>, RabbitMqMessageQueueConsumerService<OrderCreatedEvent>>();
    })
    .Build();

host.Run();
