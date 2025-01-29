using GoalManager.Core.Interfaces;
using GoalManager.Core.Notification;
using GoalManager.Core.Organisation.Events;
using MediatR;

namespace GoalManager.UseCases.Notification.Handlers;

internal sealed class EventNotificationHandler(IRepository<NotificationItem> notificationItemRepository) :
  INotificationHandler<OrganisationCreatedEvent>,
  INotificationHandler<OrganisationDeletedEvent>,
  INotificationHandler<OrganisationUpdatedEvent>,
  INotificationHandler<TeamCreatedEvent>,
  INotificationHandler<TeamDeletedEvent>,
  INotificationHandler<TeamUpdatedEvent>
{
  public Task Handle(OrganisationCreatedEvent notification, CancellationToken cancellationToken)
  {
    return AddNotificationItem(notification, cancellationToken);
  }

  public Task Handle(OrganisationDeletedEvent notification, CancellationToken cancellationToken)
  {
    return AddNotificationItem(notification, cancellationToken);
  }

  public Task Handle(OrganisationUpdatedEvent notification, CancellationToken cancellationToken)
  {
    return AddNotificationItem(notification, cancellationToken);
  }

  public Task Handle(TeamCreatedEvent notification, CancellationToken cancellationToken)
  {
    return AddNotificationItem(notification, cancellationToken);
  }

  public Task Handle(TeamDeletedEvent notification, CancellationToken cancellationToken)
  {
    return AddNotificationItem(notification, cancellationToken);
  }

  public Task Handle(TeamUpdatedEvent notification, CancellationToken cancellationToken)
  {
    return AddNotificationItem(notification, cancellationToken);
  }

  private Task AddNotificationItem(IHasNotificationText hasNotificationText, CancellationToken cancellationToken)
  {
    var notificationItemResult = NotificationItem.Create(hasNotificationText.GetNotificationText());
    if (notificationItemResult.IsSuccess)
    {
      return notificationItemRepository.AddAsync(notificationItemResult.Value, cancellationToken);
    }

    // TODO: log error
    return Task.CompletedTask;
  }
}
