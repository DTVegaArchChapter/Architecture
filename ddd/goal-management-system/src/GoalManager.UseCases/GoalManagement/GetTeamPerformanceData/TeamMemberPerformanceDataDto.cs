using GoalManager.Core.GoalManagement;

namespace GoalManager.UseCases.GoalManagement.GetTeamPerformanceData;

public sealed class TeamMemberPerformanceDataDto
{
  public int UserId { get; set; }

  public int GoalSetId { get; set; }

  public GoalSetStatus? GoalSetStatus { get; set; }

  public int Year { get; set; }

  public int TeamId { get; set; }

  public IList<GoalPerformanceDataDto> Goals { get; set; } = [];
}
