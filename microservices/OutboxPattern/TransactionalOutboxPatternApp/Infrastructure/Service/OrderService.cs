namespace TransactionalOutboxPatternApp.Infrastructure.Service;

using MessageQueue;

using Microsoft.EntityFrameworkCore;

using TransactionalOutboxPatternApp.Infrastructure.Data;
using TransactionalOutboxPatternApp.Infrastructure.Entity;
using TransactionalOutboxPatternApp.Infrastructure.Events;

public interface IOrderService
{
    Task<int> CreateOrderAsync();
}

public sealed class OrderService : IOrderService
{
    private readonly ILogger<OrderService> _logger;
    private readonly OrderDbContext _orderDbContext;
    private readonly IEventLogService _eventLogService;
    private readonly IMessageQueuePublisherService _messageQueuePublisherService;

    public OrderService(ILogger<OrderService> logger, OrderDbContext orderDbContext, IEventLogService eventLogService, IMessageQueuePublisherService messageQueuePublisherService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _orderDbContext = orderDbContext ?? throw new ArgumentNullException(nameof(orderDbContext));
        _eventLogService = eventLogService ?? throw new ArgumentNullException(nameof(eventLogService));
        _messageQueuePublisherService = messageQueuePublisherService ?? throw new ArgumentNullException(nameof(messageQueuePublisherService));
    }

    public async Task<int> CreateOrderAsync()
    {
        var order = new Order { CreateDate = DateTime.Now };

        var transactionId = Guid.NewGuid();
        var strategy = _orderDbContext.Database.CreateExecutionStrategy();
        var orderId = await strategy.ExecuteInTransactionAsync(
            _orderDbContext,
            operation: async (context, cancellationToken) =>
            {
                await context.Orders.AddAsync(order, cancellationToken).ConfigureAwait(false);
                await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

                await _eventLogService.SaveEventAsync(new OrderCreatedEvent(order.Id), transactionId);

                return order.Id;
            },
        verifySucceeded: (context, cancellationToken) => context.Orders.AsNoTracking().AnyAsync(b => b.Id == order.Id, cancellationToken: cancellationToken))
            .ConfigureAwait(false);

        await PublishEventsToMessageQueueAsync(transactionId).ConfigureAwait(false);

        return orderId;
    }

    private async Task PublishEventsToMessageQueueAsync(Guid transactionId)
    {
        var @event = await _eventLogService.RetrievePendingEventToPublishAsync(transactionId).ConfigureAwait(false);
        if (@event != null)
        {
            try
            {
                await _eventLogService.MarkEventAsInProgressAsync(@event.Id).ConfigureAwait(false);
                await _messageQueuePublisherService.PublishMessage(@event).ConfigureAwait(false);
                await _eventLogService.MarkEventAsPublishedAsync(@event.Id).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ERROR publishing event: {Id}", @event.Id);

                await _eventLogService.MarkEventAsFailedAsync(@event.Id).ConfigureAwait(false);
            }
        }
    }
}