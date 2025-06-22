using GoalManager.Core.GoalManagement;

namespace GoalManager.UseCases.GoalManagement.GetGoalSet;

public sealed class GoalSetDto
{
  private List<GoalDto>? _goals;

  public int Id { get; set; }

  public int TeamId { get; set; }

  public int UserId { get; set; }

  public GoalSetStatus? Status { get; set; }

  public List<GoalDto> Goals
  {
    get => _goals ??= [];
    set => _goals = value;
  }
}
