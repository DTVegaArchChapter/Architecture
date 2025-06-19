using GoalManager.Core.GoalManagement;
using GoalManager.UseCases.GoalManagement.GetPendingApprovalGoals;
using GoalManager.UseCases.GoalManagement.GetPendingLastApprovalGoalSets;
using GoalManager.UseCases.GoalManagement.UpdateGoalSetStatus;
using GoalManager.UseCases.GoalManagement.UpdateGoalStatus;
using GoalManager.Web.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GoalManager.Web.Pages.GoalManagement;

[Authorize]
public class PendingGoalsModel(IMediator mediator) : PageModelBase
{
  public List<PendingApprovalGoalDto> PendingGoals { get; private set; } = [];
  public List<GetPendingLastApprovalGoalSetsDto> LastPendingGoalSets { get; private set; } = [];

  public async Task<IActionResult> OnGetAsync()
  {
    var user = HttpContext.GetUserContext();

    var pendingApprovalGoalDtos = await mediator.Send(new GetPendingApprovalGoalsQuery(user.Id));
    

    PendingGoals = pendingApprovalGoalDtos
        .Where(x =>  x.GoalProgressStatus == GoalProgressStatus.WaitingForApproval)
        .ToList();

    LastPendingGoalSets = (await mediator.Send(new GetPendingLastApprovalGoalSetsQuery(user.Id))).Value;

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

  public async Task<IActionResult> OnPostLastApproveAndCalculateLastPointAsync(int goalSetId)
  {
    var command = new UpdateGoalSetStatusCommand(
        goalSetId,
        GoalSetStatus.LastApproved);

    var result = await mediator.Send(command);

    if (result.IsSuccess)
      SuccessMessages.Add("Goal Last Approved successfully");
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
