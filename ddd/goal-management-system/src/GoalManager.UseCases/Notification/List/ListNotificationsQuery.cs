namespace GoalManager.UseCases.Notification.List;

public record ListNotificationsQuery(int PageNumber, int PageSize) : IQuery<PagedResult<List<NotificationListItemDto>>>;
