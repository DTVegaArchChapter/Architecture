namespace GoalManager.Core.GoalManagement;

public record GoalValue(int MinValue, int MidValue, int MaxValue, GoalValueType GoalValueType)
{
#pragma warning disable CS8618 // Required by Entity Framework
  private GoalValue()
    : this(0, 0, 0, GoalValueType.Percentage)
  {
  }
#pragma warning restore CS8618

  public static Result<GoalValue> Create(int minValue, int midValue, int maxValue, GoalValueType goalValueType)
  {
    if (minValue >= midValue)
    {
      return Result.Error("Min value must be less than mid value");
    }

    if (midValue >= maxValue)
    {
      return Result.Error("Mid value must be less than max value");
    }

    if (goalValueType == GoalValueType.Percentage && (minValue <= 0 || minValue > 100 || midValue <= 0 || midValue > 100 || maxValue <= 0 || maxValue > 100))
    {
      return Result.Error("Values must be between 1 and 100 for percentage goal type");
    }

    return new GoalValue(minValue, midValue, maxValue, goalValueType);
  }
}
