namespace GoalManager.UseCases.GoalManagement.GetPendingApprovalGoals;
public record PendingApprovalGoalDto
{
  public int TeamId { get; set; }
  public string TeamName { get; set; } = null!;
  public int GoalId { get; set; }
  public int GoalSetId { get; set; }
  public string GoalTitle { get; set; } = null!;
  public int MinValue { get; set; }
  public int MaxValue { get; set; }
  public int GoalOwnerUserId { get; set; }
  public int ActualValue { get; set; }
  public string? Comment { get; set; }
}
