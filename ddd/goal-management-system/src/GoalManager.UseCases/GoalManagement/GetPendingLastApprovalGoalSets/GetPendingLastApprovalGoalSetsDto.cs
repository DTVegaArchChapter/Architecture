
using GoalManager.Core.GoalManagement;

namespace GoalManager.UseCases.GoalManagement.GetPendingLastApprovalGoalSets;

public class GetPendingLastApprovalGoalSetsDto
{
  public int TeamId { get; set; }

  public string? TeamName { get; set; }

  public int GoalSetId { get; set; }

  public int UserId { get; set; }

  public string? User { get; set; }

  public GoalSetStatus? Status { get; set; }
}
