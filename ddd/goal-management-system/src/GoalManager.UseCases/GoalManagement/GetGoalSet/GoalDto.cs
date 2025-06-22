using GoalManager.Core.GoalManagement;

namespace GoalManager.UseCases.GoalManagement.GetGoalSet;

public sealed class GoalDto
{
  public int Id { get; set; }

  public string Title { get; set; } = null!;

  public required GoalType GoalType { get; set; }

  public required GoalValue GoalValue { get; set; }

  public int Percentage { get; set; }

  public int? ActualValue { get; set; }

  public GoalProgressStatus? Status { get; set; }

  public string? Comment { get; set; }
}
