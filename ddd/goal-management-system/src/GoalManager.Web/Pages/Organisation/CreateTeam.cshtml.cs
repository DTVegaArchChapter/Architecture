using GoalManager.UseCases.Organisation.AddTeam;
using GoalManager.Web.Common;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;

namespace GoalManager.Web.Pages.Organisation;

[Authorize]
public class CreateTeamModel(IMediator mediator) : PageModelBase
{
  [Required]
  [BindProperty]
  public string TeamName { get; set; } = null!;

  public async Task<IActionResult> OnPostAsync(int organisationId)
  {
    if (!ModelState.IsValid)
    {
      return Page();
    }

    var result = await mediator.Send(new AddTeamCommand(organisationId, TeamName)).ConfigureAwait(false); 
    
    AddResultMessages(result);

    if (!result.IsSuccess)
    {
      return Page();
    }

    return RedirectToPageWithSuccessMessage("Update", new { id = organisationId }, $"Team '{TeamName}' is created");
  }
}
