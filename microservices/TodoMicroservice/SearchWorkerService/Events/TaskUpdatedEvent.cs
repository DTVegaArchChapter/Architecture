namespace SearchWorkerService.Events;

using MessageQueue.Events;

[EventName("taskmanagement.task.updated")]
public sealed class TaskUpdatedEvent : EventBase
{
    public int TaskId { get; set; }

    public string? Title { get; set; }
}