namespace TransactionalOutboxPatternApp.Infrastructure.Service;

using System.Text.Json;

using MessageQueue.Events;

using Microsoft.EntityFrameworkCore;

using TransactionalOutboxPatternApp.Infrastructure.Data;
using TransactionalOutboxPatternApp.Infrastructure.Entity;

public interface IEventLogService
{
    Task SaveEventAsync(EventBase @event, Guid transactionId);

    Task<EventBase?> RetrievePendingEventToPublishAsync(Guid transactionId);

    Task MarkEventAsInProgressAsync(Guid eventId);

    Task MarkEventAsPublishedAsync(Guid eventId);

    Task MarkEventAsFailedAsync(Guid eventId);
}

public sealed class EventLogService : IEventLogService
{
    private readonly OrderDbContext _orderDbContext;

    public EventLogService(OrderDbContext orderDbContext)
    {
        _orderDbContext = orderDbContext ?? throw new ArgumentNullException(nameof(orderDbContext));
    }

    public Task SaveEventAsync(EventBase @event, Guid transactionId)
    {
        var eventLogEntry = new EventLogEntry
                                {
                                    EventId = @event.Id,
                                    TransactionId = transactionId,
                                    CreateDate = @event.CreateDate,
                                    Content = JsonSerializer.Serialize(@event, @event.GetType(), new JsonSerializerOptions { WriteIndented = false }),
                                    EventTypeName = @event.GetType().FullName,
                                    State = EventLogEntryState.NotPublished
                                };
        _orderDbContext.EventLogEntries.Add(eventLogEntry);

        return _orderDbContext.SaveChangesAsync();
    }

    public async Task<EventBase?> RetrievePendingEventToPublishAsync(Guid transactionId)
    {
        var eventLogEntry = await _orderDbContext.EventLogEntries
            .SingleOrDefaultAsync(x => x.TransactionId == transactionId && x.State == EventLogEntryState.NotPublished)
            .ConfigureAwait(false);

        if (eventLogEntry == null)
        {
            return null;
        }

        return JsonSerializer.Deserialize(eventLogEntry.Content!, Type.GetType(eventLogEntry.EventTypeName!)!) as EventBase;
    }

    public Task MarkEventAsInProgressAsync(Guid eventId)
    {
        return UpdateEventStatusAsync(eventId, EventLogEntryState.InProgress);
    }

    public Task MarkEventAsPublishedAsync(Guid eventId)
    {
        return UpdateEventStatusAsync(eventId, EventLogEntryState.Published);
    }

    public Task MarkEventAsFailedAsync(Guid eventId)
    {
        return UpdateEventStatusAsync(eventId, EventLogEntryState.Failed);
    }

    private Task UpdateEventStatusAsync(Guid eventId, EventLogEntryState status)
    {
        if (status == EventLogEntryState.InProgress)
        {
            return _orderDbContext.EventLogEntries.Where(e => e.EventId == eventId)
                .ExecuteUpdateAsync(
                c => c.SetProperty(x => x.State, status)
                                                             .SetProperty(x => x.TimesSent, x => x.TimesSent + 1));
        }

        return _orderDbContext.EventLogEntries.Where(e => e.EventId == eventId)
                             .ExecuteUpdateAsync(c => c.SetProperty(x => x.State, status));
    }
}