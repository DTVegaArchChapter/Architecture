using GoalManager.UseCases.GoalManagement.AddGoalPeriod;
using GoalManager.Web.Common;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GoalManager.Web.Pages.GoalManagement;

[Authorize]
public class AddGoalPeriodModel(IMediator mediator) : PageModelBase
{
  public async Task<IActionResult> OnGetAsync(int teamId)
  {
    var result = await mediator.Send(new AddGoalPeriodCommand(teamId, 0, DateTime.Now.Year));

    AddResultMessages(result);

    return Page();
  }
}
