namespace TaskManagementApi.HostedServices;

using MessageQueue;

using Microsoft.AspNetCore.SignalR;

using TaskManagementApi.Events;
using TaskManagementApi.Hubs;

public sealed class GeneralNotificationHandler : BackgroundService
{
    private readonly IMessageQueueConsumerService<GeneralNotificationEvent> _generalNotificationConsumerService;

    private readonly IHubContext<GeneralNotificationHub> _generalNotificationHubContext;

    public GeneralNotificationHandler(
        IMessageQueueConsumerService<GeneralNotificationEvent> generalNotificationConsumerService,
        IHubContext<GeneralNotificationHub> generalNotificationHubContext)
    {
        _generalNotificationConsumerService = generalNotificationConsumerService ?? throw new ArgumentNullException(nameof(generalNotificationConsumerService));
        _generalNotificationHubContext = generalNotificationHubContext ?? throw new ArgumentNullException(nameof(generalNotificationHubContext));
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (!stoppingToken.IsCancellationRequested)
        {
            _generalNotificationConsumerService.ConsumeMessage(
                (m, _) =>
                    {
                        _generalNotificationHubContext.Clients.All.SendCoreAsync("GeneralNotification", new object[]{ m.Message! }, stoppingToken);
                        return Task.FromResult(true);
                    });
        }

        return Task.CompletedTask;
    }
}
