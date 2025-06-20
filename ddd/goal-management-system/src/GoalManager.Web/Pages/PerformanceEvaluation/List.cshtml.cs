using GoalManager.UseCases.GoalManagement.GetTeamGoalSetListsOfTeamLeader;
using GoalManager.UseCases.PerformanceEvaluation.CalculatePerformanceEvaluation;
using GoalManager.Web.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GoalManager.Web.Pages.PerformanceEvaluation;

[Authorize]
public class ListModel(IMediator mediator) : PageModelBase
{
  public List<TeamGoalSetListItem>? Items { get; private set; }

  public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken = default)
  {
    var user = HttpContext.GetUserContext();
    var result = await mediator.Send(new GetTeamGoalSetListsOfTeamLeaderQuery(user.Id), cancellationToken).ConfigureAwait(false);
    
    AddResultMessages(result);

    Items = result.IsSuccess ? result.Value : [];

    return Page();
  }

  public async Task<IActionResult> OnPostCalculateEvaluationAsync(int teamId, CancellationToken cancellationToken = default)
  {
    var result = await mediator.Send(new CalculatePerformanceEvaluationCommand(teamId), cancellationToken).ConfigureAwait(false);
    
    AddResultMessages(result);

    return await OnGetAsync(cancellationToken).ConfigureAwait(false);
  }
}
