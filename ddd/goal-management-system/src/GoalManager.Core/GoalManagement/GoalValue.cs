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
    return new GoalValue(minValue, midValue, maxValue, goalValueType);
  }
}
