using GoalManager.Core.Interfaces;

namespace GoalManager.Core.Organisation.Events;

public sealed class OrganisationUpdatedEvent(int organisationId, string organisationName) : DomainEventBase, IHasNotificationText
{
  public int OrganisationId { get; private set; } = organisationId;

  public string OrganisationName { get; private set; } = organisationName;

  public string GetNotificationText()
  {
    return $"Organisation name is updated to {OrganisationName}. Organisation Id: {OrganisationId}";
  }
}
