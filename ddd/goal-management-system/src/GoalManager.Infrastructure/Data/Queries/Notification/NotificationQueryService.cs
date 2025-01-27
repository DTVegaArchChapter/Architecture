using Ardalis.Result;

using GoalManager.UseCases.Notification;
using GoalManager.UseCases.Notification.List;

namespace GoalManager.Infrastructure.Data.Queries.Notification;

public sealed class NotificationQueryService(AppDbContext appDbContext) : INotificationQueryService
{
  public async Task<PagedResult<List<NotificationListItemDto>>> List(int pageNumber, int pageSize, CancellationToken cancellationToken)
  {
    var query = appDbContext.NotificationItem
      .Skip((pageNumber - 1) * pageSize)
      .Take(pageSize)
      .Select(x => new NotificationListItemDto(x.Text, x.CreateDate));
    var list = await query.ToListAsync(cancellationToken);
    var count = await query.CountAsync(cancellationToken);

    return new PagedResult<List<NotificationListItemDto>>(new PagedInfo(pageNumber, pageSize, (long)Math.Ceiling(count / (decimal)pageSize), count), list);
  }
}
