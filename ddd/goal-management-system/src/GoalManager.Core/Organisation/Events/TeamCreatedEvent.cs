using GoalManager.Core.Interfaces;

namespace GoalManager.Core.Organisation.Events;

public sealed class TeamCreatedEvent(string name) : DomainEventBase, IHasNotificationText
{
  public string Name { get; private set; } = name;

  public string GetNotificationText()
  {
    return $"Team with name '{Name}' is created";
  }
}
