using GoalManager.Core.Interfaces;

namespace GoalManager.Core.GoalManagement.Events;

public sealed class GoalAddedEvent(int goalSetId, string title) : DomainEventBase, IHasNotificationText
{
  public int GoalSetId { get; } = goalSetId;

  public string Title { get; } = title;

  public string GetNotificationText()
  {
    return $"Goal '{Title}' is added to the GoalSet #{GoalSetId}";
  }
}
