namespace GoalManager.Core.Notification;

public sealed class NotificationItem : EntityBase, IAggregateRoot
{
  public string Text { get; private set; } = null!;

  public DateTime CreateDate { get; private set; }

#pragma warning disable CS8618 // Required by Entity Framework
  private NotificationItem() { }
#pragma warning restore CS8618

  private NotificationItem(string text)
  {
    Text = Guard.Against.NullOrWhiteSpace(text);
    CreateDate = DateTime.Now;
  }

  public static Result<NotificationItem> Create(string text)
  {
    if (string.IsNullOrWhiteSpace(text))
    {
      return Result<NotificationItem>.Error("Notification text is required");
    }

    return new NotificationItem(text);
  }
}
