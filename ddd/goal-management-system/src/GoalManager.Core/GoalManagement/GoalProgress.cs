namespace GoalManager.Core.GoalManagement;
public class GoalProgress : EntityBase
{
  public int GoalId { get; private set; }
  public Goal Goal { get; private set; } = null!;
  public int ActualValue { get; private set; }
  public string? Comment { get; private set; }
  public GoalProgressStatus Status { get; set; } = null!;
}
