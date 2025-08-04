using GoalManager.UseCases.GoalManagement.CreateGoalSet;
using GoalManager.UseCases.GoalManagement.GetGoalSet;
using GoalManager.UseCases.GoalManagement.SendGoalSetToApproval;
using GoalManager.UseCases.GoalManagement.UpdateGoalProgress;
using GoalManager.Web.Common;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GoalManager.Web.Pages.GoalManagement;

[Authorize]
public class TeamGoalsModel(IMediator mediator) : PageModelBase
{
  public int Year { get; private set; }
  public GoalSetDto? GoalSet { get; private set; }
  public async Task<IActionResult> OnGetAsync(int teamId)
  {
    var year = DateTime.Now.Year;
    var user = HttpContext.GetUserContext();

    Year = year;

    var goalSetResult = await mediator.Send(new GetGoalSetQuery(teamId, year, user.Id)).ConfigureAwait(false);
    if (goalSetResult.IsSuccess)
    {
      if (goalSetResult.Value == null)
      {
        await mediator.Send(new CreateGoalSetCommand(teamId, year, user.Id));
        goalSetResult = await mediator.Send(new GetGoalSetQuery(teamId, year, user.Id)).ConfigureAwait(false);
      }

      GoalSet = goalSetResult.Value;
    }

    AddResultMessages(goalSetResult);

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

  public async Task<IActionResult> OnPostSendToApprovalAsync(int teamId, int goalSetId)
  {
    var result = await mediator.Send(new SendGoalSetToApprovalCommand(goalSetId)).ConfigureAwait(false);

    AddResultMessages(result);

    return await OnGetAsync(teamId).ConfigureAwait(false);
  }
}
