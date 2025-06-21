using GoalManager.Core.GoalManagement;

namespace GoalManager.UseCases.GoalManagement.GetTeamGoalSetListsOfTeamLeader;

public sealed class TeamMemberGoalSetListItemDto
{
  public int TeamId { get; set; }

  public int UserId { get; set; }

  public GoalSetStatus? Status { get; set; }
}
