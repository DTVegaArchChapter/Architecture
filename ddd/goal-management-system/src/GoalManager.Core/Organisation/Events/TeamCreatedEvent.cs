namespace GoalManager.Core.Organisation.Events;

public sealed class TeamCreatedEvent(string name) : DomainEventBase
{
  public string Name { get; private set; } = name;
}
