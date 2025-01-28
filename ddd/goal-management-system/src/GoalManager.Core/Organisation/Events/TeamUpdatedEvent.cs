using GoalManager.Core.Interfaces;

namespace GoalManager.Core.Organisation.Events;

public sealed class TeamUpdatedEvent(int id, string name) : DomainEventBase, IHasNotificationText
{
  private int Id { get; } = id;

  public string Name { get; private set; } = name;

  public string GetNotificationText()
  {
    return $"Team name is updated to {Name}. Team Id: {Id}";
  }
}
