using GoalManager.UseCases.Organisation.DeleteTeam;
using GoalManager.UseCases.Organisation.GetForUpdate;
using GoalManager.UseCases.Organisation.UpdateOrganisations;
using GoalManager.Web.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GoalManager.Web.Pages.Organisation;

[Authorize]
public class UpdateModel(IMediator mediator) : PageModelBase
{
  [BindProperty]
  public OrganisationForUpdateDto? Organisation { get; set; }

  public async Task<IActionResult> OnGetAsync(int id)
  {
    var result = await mediator.Send(new GetOrganisationForUpdateQuery(id)).ConfigureAwait(false);

    AddResultMessages(result);

    if (result.IsSuccess)
    {
      Organisation = result.Value;
    }

    return Page();
  }

  public async Task<IActionResult> OnPostAsync(int id)
  {
    if (!ModelState.IsValid || Organisation == null)
    {
      return await OnGetAsync(id).ConfigureAwait(false);
    }

    var result = await mediator.Send(new UpdateOrganisationCommand(id, Organisation.OrganisationName)).ConfigureAwait(false);

    AddResultMessages(result);

    if (!result.IsSuccess)
    {
      return await OnGetAsync(id).ConfigureAwait(false);
    }

    AddSuccessMessage("Organisation is updated");

    return await OnGetAsync(id).ConfigureAwait(false);
  }

  public async Task<IActionResult> OnPostDeleteTeamAsync(int organisationId, int teamId)
  {
    ModelState.Clear();

    var result = await mediator.Send(new DeleteTeamCommand(organisationId, teamId)).ConfigureAwait(false);

    AddResultMessages(result);

    await OnGetAsync(organisationId).ConfigureAwait(false);

    return Page();
  }
}
