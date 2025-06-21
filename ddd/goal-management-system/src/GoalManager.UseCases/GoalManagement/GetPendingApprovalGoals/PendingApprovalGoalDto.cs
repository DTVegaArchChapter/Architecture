using GoalManager.Core.GoalManagement;

namespace GoalManager.UseCases.GoalManagement.GetPendingApprovalGoals;

public record PendingApprovalGoalDto
{
  public int TeamId { get; set; }

  public string? TeamName { get; set; }

  public int GoalId { get; set; }

  public int GoalSetId { get; set; }

  public string GoalTitle { get; set; } = null!;

  public int MinValue { get; set; }

  public int MaxValue { get; set; }

  public int UserId { get; set; }

  public string? User { get; set; }

  public int ActualValue { get; set; }

  public string? Comment { get; set; }

  public GoalProgressStatus? GoalProgressStatus { get; set; }
}
