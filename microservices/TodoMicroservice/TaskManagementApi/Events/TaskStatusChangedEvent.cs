namespace TaskManagementApi.Events;

using MessageQueue.Events;

[EventName("taskmanagement.task.statuschanged")]
public sealed class TaskStatusChangedEvent : EventBase
{
    public int TaskId { get; }

    public bool Completed { get; }

    public TaskStatusChangedEvent(int taskId, bool completed)
    {
        if (taskId <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(taskId));
        }

        TaskId = taskId;
        Completed = completed;
    }
}