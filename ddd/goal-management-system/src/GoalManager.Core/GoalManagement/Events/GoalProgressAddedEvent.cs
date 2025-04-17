using GoalManager.Core.Interfaces;

namespace GoalManager.Core.GoalManagement.Events;
public sealed class GoalProgressAddedEvent(int goalId, int actualValue) : DomainEventBase, IHasNotificationText
{
  public int GoalId { get; } = goalId;

  public int ActualValue { get; } = actualValue;

  public string GetNotificationText()
  {
    return $"Goal progress is added to goal '{GoalId}' with actual value {ActualValue}";
  }
}
