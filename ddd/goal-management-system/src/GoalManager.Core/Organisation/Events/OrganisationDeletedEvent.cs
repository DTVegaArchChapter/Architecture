namespace GoalManager.Core.Organisation.Events;

internal sealed class OrganisationDeletedEvent(int organisationId, string organisationName) : DomainEventBase
{
  public int OrganisationId { get; private set; } = organisationId;

  public string OrganisationName { get; private set; } = organisationName;
}
