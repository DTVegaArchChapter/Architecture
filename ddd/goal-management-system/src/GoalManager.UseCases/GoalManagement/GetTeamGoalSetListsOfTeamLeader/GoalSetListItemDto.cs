using GoalManager.Core.GoalManagement;

namespace GoalManager.UseCases.GoalManagement.GetTeamGoalSetListsOfTeamLeader;

public sealed class GoalSetListItemDto
{
  public string User { get; set; } = null!;

  public GoalSetStatus? Status { get; set; }
}
