using GoalManager.UseCases.Contributors;
using GoalManager.UseCases.Contributors.List;
using GoalManager.Web.Contributors;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GoalManager.Web.Pages;

[Authorize]
public class IndexModel(IMediator mediator) : PageModel
{
  public ContributorListResponse? ContributorList { get; private set; }

  public IEnumerable<string>? Errors { get; private set; }

  public async Task OnGetAsync(CancellationToken cancellationToken = default)
  {
    Result<IEnumerable<ContributorDTO>> result = await mediator.Send(new ListContributorsQuery(null, null), cancellationToken);

    if (result.IsSuccess)
    {
      ContributorList = new ContributorListResponse
                        {
                          Contributors = result.Value.Select(c => new ContributorRecord(c.Id, c.Name, c.PhoneNumber)).ToList()
                        };
    }
    else
    {
      Errors = result.Errors;
    }
  }
}
