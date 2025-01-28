using GoalManager.UseCases.Notification.List;
using GoalManager.Web.Common;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GoalManager.Web.Pages.Notification;

[Authorize]
public class ListModel(IMediator mediator) : PageModelBase
{
  private const int PageSize = 10;

  public PagedResult<List<NotificationListItemDto>> NotificationsPagedResult { get; private set; } =
    new(new PagedInfo(1, PageSize, 0, 0), new List<NotificationListItemDto>(0));

  public async Task OnGetAsync([FromQuery] int? page, CancellationToken cancellationToken = default)
  {
    var result = await mediator.Send(new ListNotificationsQuery(page < 1 ? 1 : page.GetValueOrDefault(1), PageSize), cancellationToken).ConfigureAwait(false);
    AddResultMessages(result);
    NotificationsPagedResult = result;
  }
}
