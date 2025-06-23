
namespace GoalManager.Core.GoalManagement;

public class GoalSetStatus : SmartEnum<GoalSetStatus>
{
  public static readonly GoalSetStatus None = new(nameof(None), 0);
  public static readonly GoalSetStatus WaitingForApproval = new(nameof(WaitingForApproval), 1);
  public static readonly GoalSetStatus Approved = new(nameof(Approved), 2);

  protected GoalSetStatus(string name, int value) : base(name, value) { }
}
