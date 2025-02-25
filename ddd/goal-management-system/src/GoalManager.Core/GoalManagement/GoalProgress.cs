namespace GoalManager.Core.GoalManagement;
public class GoalProgress : EntityBase
{
  public int GoalId { get; private set; }
  public int ActualValue { get; private set; }
  public string? Comment { get; private set; }
  public GoalProgressStatus Status { get; set; } = null!;
}
