namespace SearchWorkerService.Events;

using MessageQueue.Events;

[EventName("taskmanagement.task.added")]
public sealed class TaskAddedEvent : EventBase
{
    public int TaskId { get; set; }

    public string? Title { get; set; }
}