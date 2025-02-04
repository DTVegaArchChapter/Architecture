namespace TransactionalOutboxPatternApp.Infrastructure.Entity;

public enum EventLogEntryState
{
    NotPublished = 0,
    InProgress = 1,
    Published = 2,
    Failed = 3
}