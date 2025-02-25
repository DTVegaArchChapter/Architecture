using GoalManager.Core.Interfaces;

namespace GoalManager.Core.GoalManagement.Events;

public sealed class GoalCreatedEvent(string title) : DomainEventBase, IHasNotificationText
{
  public string Title { get; } = title;

  public string GetNotificationText()
  {
    return $"Goal '{Title}' is created";
  }
}
