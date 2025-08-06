using GoalManager.Core.Interfaces;

namespace GoalManager.Core.GoalManagement.Events;

public sealed class GoalProgressApprovedEvent(int goalId) : DomainEventBase, IHasNotificationText
{
  public int GoalId { get; } = goalId;

  public string GetNotificationText()
  {
    return $"Goal #{GoalId} progress is marked as Approved.";
  }
}
