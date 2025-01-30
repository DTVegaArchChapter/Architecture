namespace GoalManager.Core.GoalManagement;

public class GoalType : SmartEnum<GoalType>
{
  public static readonly GoalType Team = new(nameof(Team), 1);
  public static readonly GoalType Individual = new(nameof(Individual), 2);

  protected GoalType(string name, int value) : base(name, value) { }
}
