namespace GoalManager.Core.GoalManagement;

public readonly record struct GoalValue(int minValue, int midValue, int maxValue, GoalValueType goalValueType)
{
  public int MinValue { get; private init; } = minValue;

  public int MidValue { get; private init; } = midValue;

  public int MaxValue { get; private init; } = maxValue;

  public GoalValueType GoalValueType { get; private init; } = goalValueType;

  public static Result<GoalValue> Create(int minValue, int midValue, int maxValue, GoalValueType goalValueType)
  {
    return new GoalValue(minValue, midValue, maxValue, goalValueType);
  }
}
