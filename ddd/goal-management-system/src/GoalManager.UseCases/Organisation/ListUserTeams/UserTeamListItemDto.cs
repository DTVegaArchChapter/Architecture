using GoalManager.Core.Organisation;

namespace GoalManager.UseCases.Organisation.ListUserTeams;

public sealed class UserTeamListItemDto
{
  public int TeamId { get; set; }

  public string TeamName { get; set; } = null!;

  public TeamMemberType? TeamMemberType { get; set; }
}
