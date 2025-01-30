using ElasticSearch;

using MessageQueue;

using Nest;

using SearchWorkerService;
using SearchWorkerService.Events;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((builder, services) =>
    {
        services.AddHostedService<Worker>();
        services.AddSingleton<IElasticClient>(_ => new ElasticClient(new ConnectionSettings(new Uri("http://elasticsearch:9200")).BasicAuthentication("elastic", "Password1").DefaultIndex("task").DefaultMappingFor<SearchWorkerService.Infrastructure.Model.Task>(x => x.IdProperty(y => y.Id).IndexName("task"))));
        services.AddSingleton<IElasticSearchRepository, ElasticSearchRepository>();
        services.AddSingleton<IRabbitMqConnection>(_ => new DefaultRabbitMqConnection(builder.Configuration.GetSection("RabbitMqConnection").Get<RabbitMqConnectionSettings>()));
        services.AddSingleton<IMessageQueueConsumerService<TaskAddedEvent>>(x => new RabbitMqMessageQueueConsumerService<TaskAddedEvent>(x.GetRequiredService<IRabbitMqConnection>(), builder.Configuration.GetSection("RabbitMqConsumerTaskAdded").Get<RabbitMqConsumerSettings>()));
        services.AddSingleton<IMessageQueueConsumerService<TaskStatusChangedEvent>>(x => new RabbitMqMessageQueueConsumerService<TaskStatusChangedEvent>(x.GetRequiredService<IRabbitMqConnection>(), builder.Configuration.GetSection("RabbitMqConsumerTaskStatusChanged").Get<RabbitMqConsumerSettings>()));
        services.AddSingleton<IMessageQueueConsumerService<TaskUpdatedEvent>>(x => new RabbitMqMessageQueueConsumerService<TaskUpdatedEvent>(x.GetRequiredService<IRabbitMqConnection>(), builder.Configuration.GetSection("RabbitMqConsumerTaskUpdated").Get<RabbitMqConsumerSettings>()));
        services.AddSingleton<IMessageQueueConsumerService<TaskDeletedEvent>>(x => new RabbitMqMessageQueueConsumerService<TaskDeletedEvent>(x.GetRequiredService<IRabbitMqConnection>(), builder.Configuration.GetSection("RabbitMqConsumerTaskDeleted").Get<RabbitMqConsumerSettings>()));
        services.AddSingleton<IMessageQueuePublisherService, RabbitMqMessageQueuePublisherService>();

    })
    .Build();

await host.RunAsync();
