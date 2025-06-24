using GoalManager.Core.GoalManagement;

namespace GoalManager.UseCases.GoalManagement.GetTeamGoalSetListsOfTeamLeader;

public sealed class TeamMemberGoalSetListItemDto
{
  public int GoalSetId { get; set; }

  public int TeamId { get; set; }

  public int UserId { get; set; }

  public GoalSetStatus? Status { get; set; }

  public int? GoalSetEvaluationId { get; set; }
}
