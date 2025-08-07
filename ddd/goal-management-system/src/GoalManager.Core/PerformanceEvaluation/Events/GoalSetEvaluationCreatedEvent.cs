using GoalManager.Core.Interfaces;

namespace GoalManager.Core.PerformanceEvaluation.Events;

public sealed class GoalSetEvaluationCreatedEvent(int goalSetId, int year, int userId, int teamId) : DomainEventBase, IHasNotificationText
{
  public int GoalSetId { get; } = goalSetId;

  public int Year { get; } = year;

  public int UserId { get; } = userId;

  public int TeamId { get; } = teamId;

  public string GetNotificationText()
  {
    return $"Goal set evaluation for goal set #{GoalSetId} and team #{TeamId} in year {Year} is created for user #{UserId}.";
  }
}
