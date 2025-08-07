using GoalManager.Core;
using GoalManager.Core.GoalManagement;
using GoalManager.Core.PerformanceEvaluation;
using GoalManager.Core.PerformanceEvaluation.Specifications;
using GoalManager.UseCases.GoalManagement;

namespace GoalManager.UseCases.PerformanceEvaluation.CalculatePerformanceEvaluation;

public sealed class CalculatePerformanceEvaluationCommandHandler(
  IGoalManagementQueryService goalManagementQueryService, 
  IRepository<GoalSetEvaluation> goalSetEvaluationRepository)
  : ICommandHandler<CalculatePerformanceEvaluationCommand, Result>
{
  public async Task<Result> Handle(CalculatePerformanceEvaluationCommand request, CancellationToken cancellationToken)
  {
    var teamGoalSetEvaluationsResult = await GetTeamGoalSetEvaluation(request.TeamId).ConfigureAwait(false);
    if (!teamGoalSetEvaluationsResult.IsSuccess)
    {
      return teamGoalSetEvaluationsResult.ToResult();
    }

    var goalSetEvaluations = teamGoalSetEvaluationsResult.Value;

    CalculatePerformanceGrades(goalSetEvaluations);

    await goalSetEvaluationRepository.AddRangeAsync(goalSetEvaluations.Where(x => x.Id == 0), cancellationToken).ConfigureAwait(false);

    return Result.SuccessWithMessage("Performance Evaluation calculation finished for the team.");
  }

  private async Task<Result<List<GoalSetEvaluation>>> GetTeamGoalSetEvaluation(int teamId)
  {
    var teamGoalSetEvaluations = new List<GoalSetEvaluation>();
    var data = await goalManagementQueryService.GetTeamPerformanceData(teamId).ConfigureAwait(false);

    if (data.TeamMembersPerformanceData.Any(x => x.GoalSetStatus != GoalSetStatus.Approved))
    {
      return Result.Error("Cannot calculate performance evaluation for team because not all team user goals are approved.");
    }

    foreach (var memberPerformanceData in data.TeamMembersPerformanceData)
    {
      var goalSetId = memberPerformanceData.GoalSetId;
      
      var currentGoalSetEvaluation = await goalSetEvaluationRepository.SingleOrDefaultAsync(new GoalSetEvaluationWithGoalEvaluationsByGoalSetIdSpec(goalSetId)).ConfigureAwait(false);
      if (currentGoalSetEvaluation != null)
      {
        teamGoalSetEvaluations.Add(currentGoalSetEvaluation);
        continue;
      }

      var goalEvaluations = new List<GoalEvaluation>(memberPerformanceData.Goals.Count);

      foreach (var goalPerformanceData in memberPerformanceData.Goals)
      {
        var goalEvaluationValueResult = GoalEvaluationValue.Create(
          goalPerformanceData.GoalValue.MinValue,
          goalPerformanceData.GoalValue.MidValue,
          goalPerformanceData.GoalValue.MaxValue);
        if (!goalEvaluationValueResult.IsSuccess)
        {
          return goalEvaluationValueResult.ToResult();
        }

        var goalEvaluationResult = GoalEvaluation.Create(
          0,
          goalPerformanceData.Title,
          goalEvaluationValueResult.Value,
          goalPerformanceData.ActualValue.GetValueOrDefault(),
          goalPerformanceData.Percentage);
        if (!goalEvaluationResult.IsSuccess)
        {
          return goalEvaluationResult.ToResult();
        }

        goalEvaluations.Add(goalEvaluationResult.Value);
      }

      var goalSetEvaluationResult = GoalSetEvaluation.Create(memberPerformanceData.GoalSetId, memberPerformanceData.Year, memberPerformanceData.UserId, memberPerformanceData.TeamId, goalEvaluations);
      if (!goalSetEvaluationResult.IsSuccess)
      {
        return goalSetEvaluationResult.ToResult();
      }

      teamGoalSetEvaluations.Add(goalSetEvaluationResult.Value);
    }

    return teamGoalSetEvaluations;
  }

  private static void CalculatePerformanceGrades(List<GoalSetEvaluation> goalSets)
  {
    var scores = goalSets.Select(x => x.PerformanceScore.GetValueOrDefault()).ToList();
    double avg = scores.Average();
    double stdDev = Math.Sqrt(scores.Sum(s => Math.Pow(s - avg, 2)) / scores.Count);

    foreach (var goalSet in goalSets)
    {
      var score = goalSet.PerformanceScore.GetValueOrDefault();
      double z = (score - avg) / stdDev;
      string grade = z switch
      {
        > 1.5 => "A",
        > 0.5 => "B",
        > -0.5 => "C",
        > -1.5 => "D",
        _ => "F"
      };

      goalSet.SetPerformanceGrade(grade);
    }
  }
}
