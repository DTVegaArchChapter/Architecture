namespace GoalManager.Core.Organisation.Events;

public sealed class OrganisationUpdatedEvent(int organisationId, string organisationName) : DomainEventBase
{
  public int OrganisationId { get; private set; } = organisationId;

  public string OrganisationName { get; private set; } = organisationName;
}
