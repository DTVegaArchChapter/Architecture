using GoalManager.UseCases.Organisation.List;
using GoalManager.Web.ViewModels.Organisation;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GoalManager.Web.Pages;

[Authorize]
public class IndexModel(IMediator mediator) : PageModel
{
  public IList<OrganisationListItemViewModel> Organisations { get; private set; } = new List<OrganisationListItemViewModel>();

  public IEnumerable<string>? Errors { get; private set; }

  public async Task OnGetAsync(CancellationToken cancellationToken = default)
  {
    var result = await mediator.Send(new ListOrganisationsQuery(null, null), cancellationToken).ConfigureAwait(false);

    if (result.IsSuccess)
    {
      Organisations = result.Value.Select(x => new OrganisationListItemViewModel(x.Id, x.Name, x.TeamsCount)).ToList();
    }
    else
    {
      Errors = result.Errors;
    }
  }
}
