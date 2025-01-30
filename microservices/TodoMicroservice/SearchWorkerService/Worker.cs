namespace SearchWorkerService;

using System.Reflection;

using ElasticSearch;

using MessageQueue;

using Nest;

using SearchWorkerService.Events;

public class Worker : BackgroundService
{
    private readonly IMessageQueueConsumerService<TaskAddedEvent> _taskAddedConsumerService;

    private readonly IMessageQueueConsumerService<TaskStatusChangedEvent> _taskStatusChangedConsumerService;

    private readonly IMessageQueueConsumerService<TaskUpdatedEvent> _taskUpdatedConsumerService;

    private readonly IMessageQueueConsumerService<TaskDeletedEvent> _taskDeletedConsumerService;

    private readonly IMessageQueuePublisherService _messageQueuePublisherService;

    private readonly IElasticClient _elasticClient;

    private readonly IElasticSearchRepository _elasticSearchRepository;

    private readonly ILogger<Worker> _logger;

    public Worker(
        IMessageQueueConsumerService<TaskAddedEvent> taskAddedConsumerService,
        IMessageQueueConsumerService<TaskStatusChangedEvent> taskStatusChangedConsumerService,
        IMessageQueueConsumerService<TaskUpdatedEvent> taskUpdatedConsumerService,
        IMessageQueueConsumerService<TaskDeletedEvent> taskDeletedConsumerService,
        IMessageQueuePublisherService messageQueuePublisherService,
        IElasticClient elasticClient,
        IElasticSearchRepository elasticSearchRepository,
        ILogger<Worker> logger)
    {
        _taskAddedConsumerService = taskAddedConsumerService ?? throw new ArgumentNullException(nameof(taskAddedConsumerService));
        _taskStatusChangedConsumerService = taskStatusChangedConsumerService ?? throw new ArgumentNullException(nameof(taskStatusChangedConsumerService));
        _taskUpdatedConsumerService = taskUpdatedConsumerService ?? throw new ArgumentNullException(nameof(taskUpdatedConsumerService));
        _taskDeletedConsumerService = taskDeletedConsumerService ?? throw new ArgumentNullException(nameof(taskDeletedConsumerService));
        _messageQueuePublisherService = messageQueuePublisherService ?? throw new ArgumentNullException(nameof(messageQueuePublisherService));
        _elasticClient = elasticClient ?? throw new ArgumentNullException(nameof(elasticClient));
        _elasticSearchRepository = elasticSearchRepository ?? throw new ArgumentNullException(nameof(elasticSearchRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (stoppingToken.IsCancellationRequested)
        {
            return Task.CompletedTask;
        }

        _elasticClient.Indices.Create(
            $"task-{DateTime.UtcNow:yyyy-MM-dd}",
            x => x.Map<Infrastructure.Model.Task>(m => m.AutoMap()));

        _taskAddedConsumerService.ConsumeMessage(
            async (m, _) =>
                {
                    try
                    {
                        _elasticSearchRepository.IndexDocument(new Infrastructure.Model.Task {Id = m.TaskId, Title = m.Title});
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e, $"Index document error for task {m.Id}");
                        await _messageQueuePublisherService.PublishMessage(new GeneralNotificationEvent(GetGeneralNotificationMessage(m.TaskId, "could not be added to ElasticSearch")))
                            .ConfigureAwait(false);
                        throw;
                    }

                    await _messageQueuePublisherService.PublishMessage(new GeneralNotificationEvent(GetGeneralNotificationMessage(m.TaskId, "is added to ElasticSearch")))
                        .ConfigureAwait(false);

                    return true;
                });

        _taskStatusChangedConsumerService.ConsumeMessage(
            async (m, _) =>
                {
                    var task = _elasticSearchRepository.Get<Infrastructure.Model.Task>(m.TaskId);
                    if (task == null)
                    {
                        await _messageQueuePublisherService.PublishMessage(new GeneralNotificationEvent(GetGeneralNotificationMessage(m.TaskId, "could not be found on ElasticSearch")))
                            .ConfigureAwait(false);

                        return false;
                    }

                    task.Completed = m.Completed;

                    try
                    {
                        _elasticSearchRepository.Update(m.TaskId, task);
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e, $"Document update error for task {m.Id}");
                        await _messageQueuePublisherService.PublishMessage(new GeneralNotificationEvent(GetGeneralNotificationMessage(m.TaskId, "could not be updated on ElasticSearch")))
                            .ConfigureAwait(false);
                        throw;
                    }

                    await _messageQueuePublisherService.PublishMessage(new GeneralNotificationEvent(GetGeneralNotificationMessage(m.TaskId, $"Completed property is set to {m.Completed} on ElasticSearch")))
                        .ConfigureAwait(false);

                    return true;
                });

        _taskUpdatedConsumerService.ConsumeMessage(
            async (m, _) =>
                {
                    var task = _elasticSearchRepository.Get<Infrastructure.Model.Task>(m.TaskId);
                    if (task == null)
                    {
                        await _messageQueuePublisherService.PublishMessage(new GeneralNotificationEvent(GetGeneralNotificationMessage(m.TaskId, "could not be found on ElasticSearch")))
                            .ConfigureAwait(false);
                        return false;
                    }

                    task.Title = m.Title;

                    try
                    {
                        _elasticSearchRepository.Update(m.TaskId, task);
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e, $"Document update error for task {m.Id}");
                        await _messageQueuePublisherService.PublishMessage(new GeneralNotificationEvent(GetGeneralNotificationMessage(m.TaskId, "could not be updated on ElasticSearch")))
                            .ConfigureAwait(false);
                        throw;
                    }

                    await _messageQueuePublisherService.PublishMessage(new GeneralNotificationEvent(GetGeneralNotificationMessage(m.TaskId, "is updated on ElasticSearch")))
                        .ConfigureAwait(false);

                    return true;
                });

        _taskDeletedConsumerService.ConsumeMessage(
            async (m, _) =>
                {
                    try
                    {
                        _elasticSearchRepository.Delete<Infrastructure.Model.Task>(m.TaskId);
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e, $"Document delete error for task {m.Id}");
                        await _messageQueuePublisherService.PublishMessage(new GeneralNotificationEvent(GetGeneralNotificationMessage(m.TaskId, "could not be deleted on ElasticSearch")))
                            .ConfigureAwait(false);
                        throw;
                    }

                    await _messageQueuePublisherService.PublishMessage(new GeneralNotificationEvent(GetGeneralNotificationMessage(m.TaskId, "is deleted on ElasticSearch")))
                        .ConfigureAwait(false);

                    return true;
                });

        return Task.CompletedTask;
    }

    private static string GetGeneralNotificationMessage(int taskId, string action)
    {
        return $"Task with id {taskId} {action} by {Assembly.GetExecutingAssembly().GetName().Name}";
    }
}