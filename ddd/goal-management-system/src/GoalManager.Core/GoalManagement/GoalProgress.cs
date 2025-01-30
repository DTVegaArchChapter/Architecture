namespace GoalManager.Core.GoalManagement;
internal class GoalProgress : EntityBase
{
  public int GoalId { get; private set; }
  public int ActualValue { get; private set; }
  public string? Comment { get; private set; }
  public GoalProgressStatus Status { get; set; } = null!;
}
