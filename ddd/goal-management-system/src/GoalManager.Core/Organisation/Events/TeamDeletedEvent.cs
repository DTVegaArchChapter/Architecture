namespace GoalManager.Core.Organisation.Events;

public sealed class TeamDeletedEvent(int id, string name) : DomainEventBase
{
  public int Id { get; private set; } = id;

  public string Name { get; private set; } = name;
}
