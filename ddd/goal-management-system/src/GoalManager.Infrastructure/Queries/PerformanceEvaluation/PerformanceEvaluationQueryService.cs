using GoalManager.Infrastructure.Data;
using GoalManager.UseCases.PerformanceEvaluation;
using GoalManager.UseCases.PerformanceEvaluation.GetRelatedGoalSetEvaluations;

namespace GoalManager.Infrastructure.Queries.PerformanceEvaluation;

internal sealed class PerformanceEvaluationQueryService(AppDbContext appDbContext) : IPerformanceEvaluationQueryService
{
  public Task<List<GoalSetEvaluationPairDto>> GetRelatedGoalSetEvaluationsAsync(List<int> goalSetIds, CancellationToken cancellationToken = default)
  {
    return appDbContext.GoalSetEvaluation
      .Where(x => goalSetIds.Contains(x.GoalSetId))
      .Select(x => new GoalSetEvaluationPairDto { GoalSetId = x.GoalSetId, GoalSetEvaluationId = x.Id })
      .ToListAsync(cancellationToken: cancellationToken);
  }
}
