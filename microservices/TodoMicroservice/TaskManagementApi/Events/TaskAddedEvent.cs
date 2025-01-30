namespace TaskManagementApi.Events;

using MessageQueue.Events;

[EventName("taskmanagement.task.added")]
public sealed class TaskAddedEvent : EventBase
{
    public int TaskId { get; }

    public string Title { get; }

    public TaskAddedEvent(int taskId, string title)
    {
        if (taskId <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(taskId));
        }

        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(title));
        }

        TaskId = taskId;
        Title = title;
    }
}