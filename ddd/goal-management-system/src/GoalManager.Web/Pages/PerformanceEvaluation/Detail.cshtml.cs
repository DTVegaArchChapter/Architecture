using GoalManager.UseCases.PerformanceEvaluation.GetPerformanceEvaluationReport;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GoalManager.Web.Pages.PerformanceEvaluation;

public class DetailModel(IMediator mediator) : PageModel
{
  public GoalSetEvaluationDto? GoalSetEvaluation { get; private set; }

  public async Task<IActionResult> OnGetAsync(int id)
  {
    GoalSetEvaluation = await mediator.Send(new GetPerformanceEvaluationReportQuery(id)).ConfigureAwait(false);

    return Page();
  }
}
