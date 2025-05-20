using GoalManager.Core.GoalManagement;
using GoalManager.UseCases.GoalManagement.CalculateCharacterPointCommand;
using GoalManager.UseCases.GoalManagement.GetAllGoalSetsByTeamId;
using GoalManager.Web.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace GoalManager.Web.Pages.GoalManagement;

[Authorize]
public class GoalSetPointModel(IMediator mediator) : PageModelBase
{
  [BindProperty(SupportsGet = true)]
  public int TeamId { get; set; }

  public List<GoalSet> GoalSets { get; set; } = new List<GoalSet>();
  public bool IsCanCalculate { get; set; }

  public async Task<IActionResult> OnGetAsync()
  {
    var goalSets = await mediator.Send(new GetAllGoalSetsByTeamIdQuery(TeamId));

    GoalSets = goalSets.Value;
    IsCanCalculate = !goalSets.Value.Any(x => x.Goals.Any(g => g.Point == null));


    return Page();
  }

  public async Task<IActionResult> OnPostCalculateCharacterPointAsync()
  {

    var result = await mediator.Send(new CalculateCharacterPointCommand(TeamId));

    AddResultMessages(result);
    return RedirectToPage();
    
  }
}

