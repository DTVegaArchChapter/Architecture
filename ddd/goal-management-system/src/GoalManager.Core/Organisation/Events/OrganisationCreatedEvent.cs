using GoalManager.Core.Interfaces;

namespace GoalManager.Core.Organisation.Events;

public sealed class OrganisationCreatedEvent(string organisationName) : DomainEventBase, IHasNotificationText
{
  public string OrganisationName { get; private set; } = organisationName;

  public string GetNotificationText()
  {
    return $"Organisation with name '{OrganisationName}' is created";
  }
}
