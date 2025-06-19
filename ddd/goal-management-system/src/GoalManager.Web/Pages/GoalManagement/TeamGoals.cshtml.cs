using GoalManager.Core.GoalManagement;
using GoalManager.UseCases.GoalManagement.GetGoalSet;
using GoalManager.UseCases.GoalManagement.UpdateGoalProgress;
using GoalManager.UseCases.GoalManagement.UpdateGoalSetStatus;
using GoalManager.Web.Common;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GoalManager.Web.Pages.GoalManagement;

[Authorize]
public class TeamGoalsModel(IMediator mediator) : PageModelBase
{
  public int Year { get; private set; }
  public GoalSet? GoalSet { get; private set; }
  public async Task<IActionResult> OnGetAsync(int teamId)
  {
    AddResultMessages(await GetGoalSet(teamId));
    return Page();
  }

  public async Task<IActionResult> OnPostUpdateProgressAsync(
      int teamId,
      int goalSetId,
      int goalId,
      int actualValue,
      string comment)
  {
    var result = await mediator.Send(new UpdateGoalProgressCommand(
        GoalSetId: goalSetId,
        GoalId: goalId,
        ActualValue: actualValue,
        Comment: comment
    )).ConfigureAwait(false);

    AddResultMessages(result);

    return await OnGetAsync(teamId).ConfigureAwait(false);
  }



  public async Task<IActionResult> OnPostLastUpdateOnProgressAsync(int teamId)
  {
    await GetGoalSet(teamId);

    if (GoalSet == null || GoalSet.Goals == null)
      return RedirectToPage();

    List<int> errorGoalId = [];

    var command = new UpdateGoalSetStatusCommand(
    GoalSet.Id,
    GoalSetStatus.WaitingForLastApproval);

    var result = await mediator.Send(command);

    if (errorGoalId.Count == 0)
      SuccessMessages.Add("Goal set WaitingForLastApproval successfully");
    else
    {
      ErrorMessages.Add($"Could not update the  goal set ID: {GoalSet.Id}");
    }

    return RedirectToPage();
  }


  private async Task<Result<GoalSet>> GetGoalSet(int teamId)
  {
    var year = DateTime.Now.Year;
    var user = HttpContext.GetUserContext();

    Year = year;
    var goalSetResult = await mediator.Send(new GetGoalSetQuery(teamId, year, user.Id)).ConfigureAwait(false);
    if (goalSetResult.IsSuccess)
    {
      GoalSet = goalSetResult.Value;
    }

    return goalSetResult;
  }
}
