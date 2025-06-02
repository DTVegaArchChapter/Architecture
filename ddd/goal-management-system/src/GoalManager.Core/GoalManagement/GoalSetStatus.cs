
namespace GoalManager.Core.GoalManagement;

public class GoalSetStatus : SmartEnum<GoalSetStatus>
{
  public static readonly GoalSetStatus WaitingForLastApproval = new(nameof(WaitingForLastApproval), 1);
  public static readonly GoalSetStatus LastApproved = new(nameof(LastApproved), 2);

  protected GoalSetStatus(string name, int value) : base(name, value) { }
}
