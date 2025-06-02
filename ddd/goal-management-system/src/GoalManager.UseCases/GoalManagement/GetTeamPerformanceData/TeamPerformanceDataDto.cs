namespace GoalManager.UseCases.GoalManagement.GetTeamPerformanceData;
public sealed class TeamPerformanceDataDto
{
  public int TeamId { get; set; }

  public IList<TeamMemberPerformanceDataDto> TeamMembersPerformanceData { get; set; } = [];
}
