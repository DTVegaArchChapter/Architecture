namespace GoalManager.UseCases.GoalManagement.GetPendingApprovalGoalSets;
public record PendingApprovalGoalSetDto
{
  public int TeamId { get; set; }

  public string? TeamName { get; set; }

  public int GoalSetId { get; set; }

  public int UserId { get; set; }

  public string? User { get; set; }
}
