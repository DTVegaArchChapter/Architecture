using GoalManager.Core.Interfaces;

namespace GoalManager.Core.GoalManagement.Events;

public sealed class GoalUpdatedEvent(int id, string title, GoalType goalType, GoalValue goalValue, int percentage) : DomainEventBase, IHasNotificationText
{
  public int Id { get; } = id;

  public string Title { get; } = title;

  public GoalType GoalType { get; } = goalType;

  public GoalValue GoalValue { get; } = goalValue;

  public int Percentage { get; } = percentage;

  public string GetNotificationText()
  {
    return $"#{Id} Goal '{Title}' is updated with type '{GoalType.Name}', value '{GoalValue}' and percentage '{Percentage}'";
  }
}
