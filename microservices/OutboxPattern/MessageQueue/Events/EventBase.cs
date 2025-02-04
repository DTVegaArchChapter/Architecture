namespace MessageQueue.Events;

public class EventBase
{
    public Guid Id { get; }

    public DateTime CreateDate { get; }

    protected EventBase()
    {
        Id = Guid.NewGuid();
        CreateDate = DateTime.Now;
    }
}