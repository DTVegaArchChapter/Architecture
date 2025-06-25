using GoalManager.Core.PerformanceEvaluation;

namespace GoalManager.UseCases.PerformanceEvaluation.GetPerformanceEvaluationReport;

public sealed class GoalEvaluationDto
{
  public required string GoalTitle { get; set; }

  public double? Point { get; set; }

  public int Percentage { get; set; }

  public GoalEvaluationValue GoalValue { get; set; } = null!;

  public int ActualValue { get; set; }
}
