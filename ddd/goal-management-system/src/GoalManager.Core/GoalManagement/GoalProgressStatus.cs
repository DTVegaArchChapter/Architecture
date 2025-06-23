namespace GoalManager.Core.GoalManagement;

public class GoalProgressStatus : SmartEnum<GoalProgressStatus>
{
  public static readonly GoalProgressStatus None = new(nameof(None), 0);
  public static readonly GoalProgressStatus WaitingForApproval = new(nameof(WaitingForApproval), 1);
  public static readonly GoalProgressStatus Approved = new(nameof(Approved), 2);
  public static readonly GoalProgressStatus Rejected = new(nameof(Rejected), 3);

  protected GoalProgressStatus(string name, int value) : base(name, value) { }
}

