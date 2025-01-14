namespace GoalManager.Core.Organisation.Events;

internal sealed class TeamUpdatedEvent(int id, string name) : DomainEventBase
{
  private int Id { get; } = id;

  public string Name { get; private set; } = name;
}
