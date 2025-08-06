using GoalManager.Core.Interfaces;

namespace GoalManager.Core.GoalManagement.Events;

public sealed class GoalProgressRejectedEvent(int id) : DomainEventBase, IHasNotificationText
{
  public int Id { get; } = id;

  public string GetNotificationText()
  {
    return $"Goal progress #{Id} is marked as Rejected.";
  }
}
