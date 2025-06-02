namespace GoalManager.UseCases.GoalManagement.GetTeamPerformanceData;

public sealed class TeamMemberPerformanceDataDto
{
  public int UserId { get; set; }

  public int GoalSetId { get; set; }

  public IList<GoalPerformanceDataDto> Goals { get; set; } = [];
}
