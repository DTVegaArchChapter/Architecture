namespace GoalManager.Core.Organisation.Events;

internal sealed class OrganisationCreatedEvent(string organisationName) : DomainEventBase
{
  public string OrganisationName { get; private set; } = organisationName;
}
