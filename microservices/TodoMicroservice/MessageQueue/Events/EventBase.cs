namespace MessageQueue.Events;

public class EventBase
{
    public Guid Id { get; }

    public DateTime CreateDate { get; }

    protected EventBase()
    {
        Id = Guid.NewGuid();
        CreateDate = DateTime.UtcNow;
    }

    protected EventBase(Guid id, DateTime createDate)
    {
        Id = id;
        CreateDate = createDate;
    }
}