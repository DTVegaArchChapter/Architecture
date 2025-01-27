using GoalManager.UseCases.Notification.List;

namespace GoalManager.UseCases.Notification;

public interface INotificationQueryService
{
  Task<PagedResult<List<NotificationListItemDto>>> List(int pageNumber, int pageSize, CancellationToken cancellationToken);
}
