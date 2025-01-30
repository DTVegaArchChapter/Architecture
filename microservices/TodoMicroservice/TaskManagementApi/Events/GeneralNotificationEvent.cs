namespace TaskManagementApi.Events;

using MessageQueue.Events;

[EventName("infrastructure.general.notification")]
public class GeneralNotificationEvent : EventBase
{
    public string? Message { get; set; }
}