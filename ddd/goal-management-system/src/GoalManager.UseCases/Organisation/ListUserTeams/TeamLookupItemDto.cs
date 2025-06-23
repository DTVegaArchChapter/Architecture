namespace GoalManager.UseCases.Organisation.ListUserTeams;

public sealed class TeamLookupItemDto
{
  public int TeamId { get; set; }

  public string TeamName { get; set; } = null!;
}
