﻿using GoalManager.UseCases.GoalManagement.ApproveGoalSet;
using GoalManager.UseCases.GoalManagement.GetGoalSet;
using GoalManager.UseCases.GoalManagement.RejectGoalSet;
using GoalManager.Web.Common;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GoalManager.Web.Pages.GoalManagement;

[Authorize]
public class ApproveTeamGoalsModel(IMediator mediator) : PageModelBase
{
  public int Year { get; private set; }

  public GoalSetDto? GoalSet { get; private set; }

  public async Task<IActionResult> OnGetAsync(int teamId, int userId)
  {
    var year = DateTime.Now.Year;

    Year = year;

    var goalSetResult = await mediator.Send(new GetGoalSetQuery(teamId, year, userId)).ConfigureAwait(false);
    if (goalSetResult.IsSuccess)
    {
      GoalSet = goalSetResult.Value;
    }

    AddResultMessages(goalSetResult);

    return Page();
  }

  public async Task<IActionResult> OnPostApproveAsync(int teamId, int userId, int goalSetId)
  {
    var result = await mediator.Send(new ApproveGoalSetCommand(goalSetId)).ConfigureAwait(false);

    AddResultMessages(result);

    return await OnGetAsync(teamId, userId).ConfigureAwait(false);
  }

  public async Task<IActionResult> OnPostRejectAsync(int teamId, int userId, int goalSetId)
  {
    var result = await mediator.Send(new RejectGoalSetCommand(goalSetId)).ConfigureAwait(false);

    AddResultMessages(result);

    return await OnGetAsync(teamId, userId).ConfigureAwait(false);
  }
}
