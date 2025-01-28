using GoalManager.Core.Interfaces;

namespace GoalManager.Core.Organisation.Events;

public sealed class TeamDeletedEvent(int id, string name) : DomainEventBase, IHasNotificationText
{
  public int Id { get; private set; } = id;

  public string Name { get; private set; } = name;

  public string GetNotificationText()
  {
    return $"Team with name '{Name}' is deleted";
  }
}
