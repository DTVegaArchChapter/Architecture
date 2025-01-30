namespace SearchWorkerService.Events;

using MessageQueue.Events;

[EventName("taskmanagement.task.statuschanged")]
public sealed class TaskStatusChangedEvent : EventBase
{
    public int TaskId { get; set; }

    public bool Completed { get; set; }
}