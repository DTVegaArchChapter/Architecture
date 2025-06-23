using GoalManager.Core.GoalManagement;

namespace GoalManager.UseCases.GoalManagement.GetTeamPerformanceData;

public sealed class GoalPerformanceDataDto
{
  public string Title { get; set; } = null!;

  public GoalType GoalType { get; set; } = null!;

  public GoalValue GoalValue { get; set; } = null!;

  public int? ActualValue { get; set; } = null!;

  public int Percentage { get; set; }
}
