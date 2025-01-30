namespace AuditLogWorkerService.Events;

using MessageQueue.Events;

[EventName("infrastructure.general.notification")]
public class GeneralNotificationEvent : EventBase
{
    public string Message { get; }

    public GeneralNotificationEvent(string message)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(message));
        }

        Message = message;
    }
}