using GoalManager.UseCases.PerformanceEvaluation.GetRelatedGoalSetEvaluations;

namespace GoalManager.UseCases.PerformanceEvaluation;

public interface IPerformanceEvaluationQueryService
{
  Task<List<GoalSetEvaluationPairDto>> GetRelatedGoalSetEvaluationsAsync(List<int> goalSetIds, CancellationToken cancellationToken = default);
}
