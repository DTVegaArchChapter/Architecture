using GoalManager.Core;
using GoalManager.Core.PerformanceEvaluation;
using GoalManager.UseCases.GoalManagement;

namespace GoalManager.UseCases.PerformanceEvaluation.CalculatePerformanceEvaluation;

public sealed class CalculatePerformanceEvaluationCommandHandler(IGoalManagementQueryService goalManagementQueryService) : ICommandHandler<CalculatePerformanceEvaluationCommand, Result>
{
  public async Task<Result> Handle(CalculatePerformanceEvaluationCommand request, CancellationToken cancellationToken)
  {
    var teamGoalSetEvaluationsResult = await GetTeamGoalSetEvaluation(request.TeamId).ConfigureAwait(false);
    if (!teamGoalSetEvaluationsResult.IsSuccess)
    {
      return teamGoalSetEvaluationsResult.ToResult();
    }

    foreach (var teamGoalSetEvaluation in teamGoalSetEvaluationsResult.Value)
    {
      teamGoalSetEvaluation.CalculatePerformancePoint();
    }

    return Result.Success();
  }

  private async Task<Result<List<GoalSetEvaluation>>> GetTeamGoalSetEvaluation(int teamId)
  {
    var teamGoalSetEvaluations = new List<GoalSetEvaluation>();
    var data = await goalManagementQueryService.GetTeamPerformanceData(teamId).ConfigureAwait(false);

    foreach (var memberPerformanceData in data.TeamMembersPerformanceData)
    {
      var goalEvaluations = new List<GoalEvaluation>(memberPerformanceData.Goals.Count);

      foreach (var goalPerformanceData in memberPerformanceData.Goals)
      {
        var goalEvaluationValueResult = GoalEvaluationValue.Create(goalPerformanceData.GoalValue.MinValue, goalPerformanceData.GoalValue.MidValue, goalPerformanceData.GoalValue.MaxValue);
        if (!goalEvaluationValueResult.IsSuccess)
        {
          return goalEvaluationValueResult.ToResult();
        }

        var goalEvaluationResult = GoalEvaluation.Create(0, goalPerformanceData.Title, goalEvaluationValueResult.Value, goalPerformanceData.ActualValue.GetValueOrDefault(), goalPerformanceData.Percentage);
        if (!goalEvaluationResult.IsSuccess)
        {
          return goalEvaluationResult.ToResult();
        }

        goalEvaluations.Add(goalEvaluationResult.Value);
      }

      var goalSetEvaluationResult = GoalSetEvaluation.Create(memberPerformanceData.GoalSetId, goalEvaluations);
      if (!goalSetEvaluationResult.IsSuccess)
      {
        return goalSetEvaluationResult.ToResult();
      }

      teamGoalSetEvaluations.Add(goalSetEvaluationResult.Value);
    }

    return teamGoalSetEvaluations;
  }
}
