using GoalManager.UseCases.Organisation.ListUserTeams;
using GoalManager.Web.Common;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GoalManager.Web.Pages.GoalManagement;

[Authorize]
public class ListTeamsModel(IMediator mediator) : PageModelBase
{
  public List<UserTeamListItemDto> Teams { get; private set; } = new();

  public int Year { get; private set; }

  public async Task<IActionResult> OnGetAsync()
  {
    var user = HttpContext.GetUserContext();
   
    Teams = await mediator.Send(new ListUserTeamsQuery(user.Id));
    Year = DateTime.Today.Year;

    return Page();
  }
}
