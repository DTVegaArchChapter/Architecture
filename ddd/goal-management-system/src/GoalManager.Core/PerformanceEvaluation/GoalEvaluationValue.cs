namespace GoalManager.Core.PerformanceEvaluation;

public record GoalEvaluationValue(int MinValue, int MidValue, int MaxValue)
{
#pragma warning disable CS8618 // Required by Entity Framework
  private GoalEvaluationValue()
    : this(0, 0, 0)
  {
  }
#pragma warning restore CS8618

  public static Result<GoalEvaluationValue> Create(int minValue, int midValue, int maxValue)
  {
    if (minValue >= midValue)
    {
      return Result.Error("Min value must be less than mid value");
    }

    if (midValue >= maxValue)
    {
      return Result.Error("Mid value must be less than max value");
    }

    return new GoalEvaluationValue(minValue, midValue, maxValue);
  }
}
