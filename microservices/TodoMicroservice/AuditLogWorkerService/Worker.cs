namespace AuditLogWorkerService
{
    using System.Reflection;
    using System.Text;

    using AuditLogWorkerService.Events;
    using AuditLogWorkerService.Infrastructure.Data;

    using MessageQueue;

    using MongoDB.Bson;
    using MongoDB.Bson.Serialization;

    public class Worker : BackgroundService
    {
        private readonly IMessageQueueConsumerService _messageQueueConsumerService;

        private readonly IRepository _repository;

        private readonly IMessageQueuePublisherService _messageQueuePublisherService;

        public Worker(
            IMessageQueueConsumerService messageQueueConsumerService,
            IRepository repository,
            IMessageQueuePublisherService messageQueuePublisherService)
        {
            _messageQueueConsumerService = messageQueueConsumerService ?? throw new ArgumentNullException(nameof(messageQueueConsumerService));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _messageQueuePublisherService = messageQueuePublisherService ?? throw new ArgumentNullException(nameof(messageQueuePublisherService));
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (!stoppingToken.IsCancellationRequested)
            {
                _messageQueueConsumerService.ConsumeMessage(
                    async (m, t) =>
                        {
                            _repository.Insert(t, BsonSerializer.Deserialize<BsonDocument>(m), cancellationToken: stoppingToken);

                            await _messageQueuePublisherService.PublishMessage(new GeneralNotificationEvent($"'{t}' event data is inserted into MongoDb by {Assembly.GetExecutingAssembly().GetName().Name}"))
                                .ConfigureAwait(false);

                            return true;
                        });
            }

            return Task.CompletedTask;
        }
    }
}