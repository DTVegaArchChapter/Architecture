using GoalManager.Core.Interfaces;

namespace GoalManager.Core.GoalManagement.Events;

public sealed class GoalSetCreatedEvent(int teamId, int periodId, int userId) : DomainEventBase, IHasNotificationText
{
  public int TeamId { get; } = teamId;

  public int PeriodId { get; } = periodId;

  public int UserId { get; } = userId;

  public string GetNotificationText()
  {
    return $"GoalSet is created for Team #{TeamId} for Period #{PeriodId} by User #{UserId}";
  }
}
