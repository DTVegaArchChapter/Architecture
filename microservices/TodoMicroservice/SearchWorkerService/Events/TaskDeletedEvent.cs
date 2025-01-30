namespace SearchWorkerService.Events;

using MessageQueue.Events;

[EventName("taskmanagement.task.deleted")]
public sealed class TaskDeletedEvent : EventBase
{
    public int TaskId { get; }

    public TaskDeletedEvent(int taskId)
    {
        if (taskId <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(taskId));
        }

        TaskId = taskId;
    }
}