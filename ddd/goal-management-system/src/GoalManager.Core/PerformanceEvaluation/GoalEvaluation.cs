namespace GoalManager.Core.PerformanceEvaluation;

public class GoalEvaluation : EntityBase
{
  public string GoalTitle { get; private set; }

  public double? Point { get; set; }

  public int Percentage { get; private set; }

  public GoalEvaluationValue GoalValue { get; private set; }

  public int ActualValue { get; private set; }

  public int GoalSetEvaluationId { get; private set; }

#pragma warning disable CS8618 // Required by Entity Framework
  private GoalEvaluation() { }
#pragma warning restore CS8618

  private GoalEvaluation(int goalSetEvaluationId, string goalTitle, GoalEvaluationValue goalValue, int actualValue, int percentage)
  {
    GoalTitle = Guard.Against.NullOrWhiteSpace(goalTitle);
    GoalValue = goalValue;
    ActualValue = actualValue;
    GoalSetEvaluationId = goalSetEvaluationId;
    Percentage = percentage;
  }

  public static Result<GoalEvaluation> Create(int goalSetEvaluationId, string goalTitle, GoalEvaluationValue goalValue, int actualValue, int percentage)
  {
    if (string.IsNullOrWhiteSpace(goalTitle))
    {
      return Result<GoalEvaluation>.Error("Goal title is required");
    }

    return new GoalEvaluation(goalSetEvaluationId, goalTitle, goalValue, actualValue, percentage);
  }

  public Result CalculatePoint()
  {
    Point = CalculatePoint(ActualValue, GoalValue.MinValue, GoalValue.MidValue, GoalValue.MaxValue);

    return Result.Success();
  }

  private static double CalculatePoint(int? actual, double min, double mid, double max)
  {
    if (actual == null)
    {
      return 0;
    }

    double actualValue = actual.Value;

    if (actualValue < min)
    {
      return 0;
    }

    if (Math.Abs(actualValue - min) == 0.0)
    {
      return 60;
    }

    if (actualValue > min && actualValue < mid) // linear calculation
    {
      return 60 + ((actualValue - min) / (mid - min)) * (80 - 60);
    }

    if (Math.Abs(actualValue - mid) == 0.0)
    {
      return 80;
    }

    if (actualValue > mid && actualValue < max) // linear calculation
    {
      return 80 + ((actualValue - mid) / (max - mid)) * (100 - 80);
    }

    return 100;
  }
}
