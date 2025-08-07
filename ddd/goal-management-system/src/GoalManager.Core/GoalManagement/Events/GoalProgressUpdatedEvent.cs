using GoalManager.Core.Interfaces;

namespace GoalManager.Core.GoalManagement.Events;
public sealed class GoalProgressUpdatedEvent(int teamId, int goalId, string goalName, int userId, int actualValue) : DomainEventBase, IHasNotificationText
{
  public int TeamId { get; } = teamId;

  public int GoalId { get; } = goalId;

  public string GoalName { get; } = goalName;

  public int UserId { get; } = userId;

  public int ActualValue { get; } = actualValue;

  public string GetNotificationText()
  {
    return $"Goal progress is added to goal '{GoalId}' with actual value {ActualValue}";
  }
}
