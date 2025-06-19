using GoalManager.Core.GoalManagement;
using GoalManager.Core.Organisation;
using GoalManager.UseCases.GoalManagement.GetAllGoalSetsByTeamId;
using GoalManager.UseCases.Organisation.ListUserTeams;
using GoalManager.Web.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GoalManager.Web.Pages.GoalManagement;

[Authorize]
public class GoalSetPointModel(IMediator mediator) : PageModelBase
{
  [BindProperty(SupportsGet = true)]
  public int TeamId { get; set; }

  public List<GoalSet> GoalSets { get; set; } = [];
  public bool IsCanCalculate { get; set; }
  public bool IsTeamLead { get; set; }
  public async Task<IActionResult> OnGetAsync()
  {
    var goalSets = await mediator.Send(new GetAllGoalSetsByTeamIdQuery(TeamId));


    var user = HttpContext.GetUserContext();
    var userTeams = await mediator.Send(new ListUserTeamsQuery(user.Id));
    var isTeamLead = userTeams.Any(x => x.TeamMemberType == TeamMemberType.TeamLeader && x.TeamId == TeamId);

    IsTeamLead = isTeamLead;
    if (IsTeamLead)
    {
      GoalSets = goalSets.Value;
    }
    else
    {
      GoalSets = goalSets.Value.Where(x => x.UserId == user.Id).ToList();
      IsCanCalculate = false;
    }

    return Page();
  }

  public IActionResult OnPostCalculateCharacterPoint()
  {
    return RedirectToPage();
  }
}

