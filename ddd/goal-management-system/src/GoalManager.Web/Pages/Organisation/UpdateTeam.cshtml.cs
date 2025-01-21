using GoalManager.UseCases.Organisation.GetTeamForUpdate;
using GoalManager.UseCases.Organisation.RemoveTeamMember;
using GoalManager.UseCases.Organisation.UpdateTeam;
using GoalManager.Web.Common;

using Microsoft.AspNetCore.Mvc;

namespace GoalManager.Web.Pages.Organisation;

public class UpdateTeamModel(IMediator mediator) : PageModelBase
{
  [BindProperty]
  public string TeamName { get; set; } = null!;

  public TeamForUpdateDto? Team { get; set; }

  public async Task<IActionResult> OnGetAsync(int teamId)
  {
    var result = await mediator.Send(new GetTeamForUpdateQuery(teamId)).ConfigureAwait(false);

    AddResultMessages(result);

    if (result.IsSuccess)
    {
      Team = result.Value;
      TeamName = result.Value.Name;
    }

    return Page();
  }

  public async Task<IActionResult> OnPostAsync(int organisationId, int teamId)
  {
    if (!ModelState.IsValid || string.IsNullOrWhiteSpace(TeamName))
    {
      return await OnGetAsync(teamId).ConfigureAwait(false);
    }

    var result = await mediator.Send(new UpdateTeamCommand(organisationId, teamId, TeamName)).ConfigureAwait(false);

    AddResultMessages(result);

    if (!result.IsSuccess)
    {
      return await OnGetAsync(teamId).ConfigureAwait(false);
    }

    AddSuccessMessage("Team is updated");

    return await OnGetAsync(teamId).ConfigureAwait(false);
  }

  public async Task<IActionResult> OnPostRemoveMemberAsync(int organisationId, int teamId, int userId)
  {
    ModelState.Clear();

    var result = await mediator.Send(new RemoveTeamMemberCommand(organisationId, teamId, userId)).ConfigureAwait(false);

    AddResultMessages(result);

    await OnGetAsync(organisationId).ConfigureAwait(false);

    return Page();
  }
}
