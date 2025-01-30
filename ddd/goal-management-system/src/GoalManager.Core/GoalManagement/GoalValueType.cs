namespace GoalManager.Core.GoalManagement;

public class GoalValueType : SmartEnum<GoalValueType>
{
  public static readonly GoalValueType Percentage = new(nameof(Percentage), 1);
  public static readonly GoalValueType Number = new(nameof(Number), 2);

  protected GoalValueType(string name, int value) : base(name, value) { }
}
