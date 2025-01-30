namespace TaskManagementApi.Events;

using MessageQueue.Events;

[EventName("taskmanagement.task.updated")]
public sealed class TaskUpdatedEvent : EventBase
{
    public int TaskId { get; set; }

    public string? Title { get; set; }

    public TaskUpdatedEvent(int taskId, string? title)
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