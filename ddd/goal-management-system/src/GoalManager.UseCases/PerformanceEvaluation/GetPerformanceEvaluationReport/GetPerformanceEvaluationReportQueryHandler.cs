using GoalManager.UseCases.Identity;
using GoalManager.UseCases.Organisation;

namespace GoalManager.UseCases.PerformanceEvaluation.GetPerformanceEvaluationReport;

internal sealed class GetPerformanceEvaluationReportQueryHandler(
  IPerformanceEvaluationQueryService performanceEvaluationQueryService,
  IOrganisationQueryService organisationQueryService,
  IIdentityQueryService identityQueryService)
  : IQueryHandler<GetPerformanceEvaluationReportQuery, GoalSetEvaluationDto?>
{
  public async Task<GoalSetEvaluationDto?> Handle(GetPerformanceEvaluationReportQuery request, CancellationToken cancellationToken)
  {
    var goalSetEvaluation = await performanceEvaluationQueryService.GetGoalSetEvaluationAsync(request.GoalSetId, cancellationToken).ConfigureAwait(false);

    if (goalSetEvaluation != null)
    {
      goalSetEvaluation.User = await identityQueryService.GetUserName(goalSetEvaluation.UserId).ConfigureAwait(false);
      goalSetEvaluation.TeamName = await organisationQueryService.GetTeamNameAsync(goalSetEvaluation.TeamId).ConfigureAwait(false);
    }

    return goalSetEvaluation;
  }
}
