using GoalManager.Core.Interfaces;

namespace GoalManager.Core.GoalManagement.Events;

public sealed class GoalPeriodCreatedEvent(int teamId, int year) : DomainEventBase, IHasNotificationText
{
  public int TeamId { get; } = teamId;

  public int Year { get; } = year;

  public string GetNotificationText()
  {
    return $"Goal period is created for teamId: {TeamId}, year: {Year}";
  }
}
