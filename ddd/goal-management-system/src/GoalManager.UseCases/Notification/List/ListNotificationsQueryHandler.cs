namespace GoalManager.UseCases.Notification.List;

internal sealed class ListNotificationsQueryHandler(INotificationQueryService notificationQueryService) : IQueryHandler<ListNotificationsQuery, PagedResult<List<NotificationListItemDto>>>
{
  public Task<PagedResult<List<NotificationListItemDto>>> Handle(ListNotificationsQuery request, CancellationToken cancellationToken)
  {
    return notificationQueryService.List(request.PageNumber, request.PageSize, cancellationToken);
  }
}
