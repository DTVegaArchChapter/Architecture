using GoalManager.Core.GoalManagement;
using GoalManager.Core.Organisation;
using GoalManager.UseCases.GoalManagement.CalculateGoalPointCommand;
using GoalManager.UseCases.GoalManagement.GetPendingApprovalGoals;
using GoalManager.UseCases.GoalManagement.UpdateGoalStatusCommand;
using GoalManager.UseCases.Organisation.ListUserTeams;
using GoalManager.Web.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GoalManager.Web.Pages.GoalManagement;

[Authorize]
public class PendingGoalsModel(IMediator mediator) : PageModelBase
{
  public List<PendingApprovalGoalDto> PendingGoals { get; private set; } = [];
  public List<PendingApprovalGoalDto> LastPendingGoals { get; private set; } = [];
  public List<PendingApprovalGoalDto> LastApprovedGoals { get; private set; } = [];

  public async Task<IActionResult> OnGetAsync()
  {
    var user = HttpContext.GetUserContext();

    var pendingApprovalGoalDtos = await mediator.Send(new GetPendingApprovalGoalsQuery(user.Id));
    var userTeams = await mediator.Send(new ListUserTeamsQuery(user.Id));
    var leaderOwnTeamsId = userTeams.Where(x => x.TeamMemberType == TeamMemberType.TeamLeader).Select(x => x.TeamId);


    PendingGoals = pendingApprovalGoalDtos
        .Where(x =>  x.GoalProgressStatus == GoalProgressStatus.WaitingForApproval)
        .ToList();

    LastPendingGoals = pendingApprovalGoalDtos
        .Where(x => leaderOwnTeamsId.Contains(x.TeamId) && x.GoalProgressStatus == GoalProgressStatus.WaitingForLastApproval)
        .ToList();

    LastApprovedGoals = pendingApprovalGoalDtos
     .Where(x => leaderOwnTeamsId.Contains(x.TeamId) && x.GoalProgressStatus == GoalProgressStatus.LastApproved)
     .ToList();


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

  public async Task<IActionResult> OnPostLastApproveAndCalculateLastPointAsync(int goalSetId, int goalId)
  {
    var command = new UpdateGoalStatusCommand(
        goalSetId,
        goalId,
        GoalProgressStatus.LastApproved);

    var result = await mediator.Send(command);

    if (result.IsSuccess)
      SuccessMessages.Add("Goal Last Approved successfully");
    else
      ErrorMessages.Add("Couldnt Update");

    var pointCommand = new CalculateGoalPointCommand(
        goalSetId,
        goalId);

    var pointResult = await mediator.Send(pointCommand);

    if (pointResult.IsSuccess)
      SuccessMessages.Add("Goal point successfully set");
    else
      ErrorMessages.Add("Couldnt set point");

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
