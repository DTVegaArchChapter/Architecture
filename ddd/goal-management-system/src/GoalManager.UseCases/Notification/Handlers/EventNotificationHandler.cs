using GoalManager.Core.Interfaces;
using GoalManager.Core.Notification;

using MediatR;

namespace GoalManager.UseCases.Notification.Handlers;

internal sealed class EventNotificationHandler<TEvent>(IRepository<NotificationItem> notificationItemRepository) : INotificationHandler<TEvent> 
  where TEvent : DomainEventBase, IHasNotificationText
{
  public Task Handle(TEvent notification, CancellationToken cancellationToken)
  {
    var notificationItemResult = NotificationItem.Create(notification.GetNotificationText());
    if (notificationItemResult.IsSuccess)
    {
      return notificationItemRepository.AddAsync(notificationItemResult.Value, cancellationToken);
    }

    // TODO: log error
    return Task.CompletedTask;
  }
}
