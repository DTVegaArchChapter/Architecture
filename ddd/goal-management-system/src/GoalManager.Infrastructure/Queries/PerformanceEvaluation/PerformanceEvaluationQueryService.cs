using GoalManager.Infrastructure.Data;
using GoalManager.UseCases.PerformanceEvaluation;
using GoalManager.UseCases.PerformanceEvaluation.GetPerformanceEvaluationReport;
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

  public Task<GoalSetEvaluationDto?> GetGoalSetEvaluationAsync(int goalSetId, CancellationToken cancellationToken)
  {
    return appDbContext.GoalSetEvaluation
      .Where(x => x.GoalSetId == goalSetId)
      .Select(x => new GoalSetEvaluationDto
      {
        Id = x.Id,
        UserId = x.UserId,
        TeamId = x.TeamId,
        GoalSetId = x.GoalSetId,
        PerformanceScore = x.PerformanceScore,
        PerformanceGrade = x.PerformanceGrade,
        Year = x.Year,
        GoalEvaluations = x.GoalEvaluations.Select(ge => new GoalEvaluationDto
        {
          GoalTitle = ge.GoalTitle,
          GoalValue = ge.GoalValue,
          Percentage = ge.Percentage,
          Point = ge.Point,
          ActualValue = ge.ActualValue
        }).ToList()
      })
      .FirstOrDefaultAsync(cancellationToken: cancellationToken);
  }
}
