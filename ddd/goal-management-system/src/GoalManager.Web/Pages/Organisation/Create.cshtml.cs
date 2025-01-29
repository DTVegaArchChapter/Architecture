using GoalManager.UseCases.Organisation.CreateOrganisation;
using GoalManager.Web.Common;
using GoalManager.Web.ViewModels.Organisation;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GoalManager.Web.Pages.Organisation;

[Authorize]
public class CreateModel(IMediator mediator) : PageModelBase
{
  [BindProperty]
  public CreateOrganisationViewModel Organisation { get; set; } = null!;

  public IActionResult OnGet()
  {
    Organisation = new CreateOrganisationViewModel();
    return Page();
  }

  public async Task<IActionResult> OnPostAsync()
  {
    if (!ModelState.IsValid)
    {
      return Page();
    }

    var result = await mediator.Send(new CreateOrganisationCommand(Organisation.Name)).ConfigureAwait(false);

    AddResultMessages(result);

    if (!result.IsSuccess)
    {
      return Page();
    }

    return RedirectToPageWithSuccessMessage("List", $"Organisation '{Organisation.Name}' is created");
  }
}
