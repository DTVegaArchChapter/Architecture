using GoalManager.Core.GoalManagement;
using GoalManager.UseCases.GoalManagement.GetPendingApprovalGoals;
using GoalManager.UseCases.GoalManagement.UpdateGoalStatusCommand;
using GoalManager.Web.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GoalManager.Web.Pages.GoalManagement;

[Authorize]
public class PendingGoalsModel(IMediator mediator) : PageModelBase
{
  public List<PendingApprovalGoalDto> PendingGoals { get; private set; } = [];

  public async Task<IActionResult> OnGetAsync()
  {
    var user = HttpContext.GetUserContext();

    PendingGoals = await mediator.Send(new GetPendingApprovalGoalsQuery(user.Id));

    return Page();
  }
  public async Task<IActionResult> OnPostApproveAsync(int goalSetId, int goalId)
  {
    var command = new UpdateGoalStatusCommand(
        goalSetId,
        goalId,
        GoalProgressStatus.Approved);

    var result = await mediator.Send(command);

    if (result.IsSuccess)
      SuccessMessages.Add("Goal approved successfully");
    else
      ErrorMessages.Add("Couldnt Update");

    return RedirectToPage();
  }

  public async Task<IActionResult> OnPostRejectAsync(int goalSetId, int goalId)
  {
    var command = new UpdateGoalStatusCommand(
        goalSetId,
        goalId,
        GoalProgressStatus.Rejected);

    var result = await mediator.Send(command);

    if (result.IsSuccess)
      SuccessMessages.Add("Goal rejected");
    else
      ErrorMessages.Add("Couldnt Update");

    return RedirectToPage();
  }
}
