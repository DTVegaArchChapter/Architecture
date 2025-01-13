namespace GoalManager.Core.Organisation.Events;

internal sealed class TeamCreatedEvent(string name) : DomainEventBase
{
  public string Name { get; private set; } = name;
}
