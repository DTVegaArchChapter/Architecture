namespace GoalManager.Core.Organisation.Events;

public sealed class TeamUpdatedEvent(int id, string name) : DomainEventBase
{
  private int Id { get; } = id;

  public string Name { get; private set; } = name;
}
