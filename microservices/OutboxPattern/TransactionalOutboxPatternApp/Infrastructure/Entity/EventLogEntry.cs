namespace TransactionalOutboxPatternApp.Infrastructure.Entity;

public sealed class EventLogEntry
{
    public Guid EventId { get; set; }

    public Guid TransactionId { get; set; }

    public string? EventTypeName { get; set; }

    public EventLogEntryState State { get; set; }

    public int TimesSent { get; set; }

    public DateTime CreateDate { get; set; }

    public string? Content { get; set; }
}