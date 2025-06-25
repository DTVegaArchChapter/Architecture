using GoalManager.Core.GoalManagement;

namespace GoalManager.UseCases.GoalManagement.GetTeamGoalSetListsOfTeamLeader;

public sealed class GoalSetListItemDto
{
  public int Id { get; set; }

  public int? GoalSetEvaluationId { get; set; }

  public string User { get; set; } = null!;

  public GoalSetStatus? Status { get; set; }
}
