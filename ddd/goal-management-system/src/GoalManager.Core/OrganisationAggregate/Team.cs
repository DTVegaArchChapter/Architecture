namespace GoalManager.Core.OrganisationAggregate;

public class Team(string name) : EntityBase
{
  private const int MaxTeamMemberCount = 10;

  public string Name { get; } = Guard.Against.NullOrWhiteSpace(name);

  public ICollection<TeamMember> TeamMembers { get; } = new List<TeamMember>();

  public Result AddTeamMember(string name, int userId)
  {
    Guard.Against.NegativeOrZero(userId);

    if (TeamMembers.Count >= MaxTeamMemberCount)
    {
      return Result.Error($"Team member count cannot be bigger than {MaxTeamMemberCount}");
    }

    if (TeamMembers.Any(x => x.UserId == userId))
    {
      return Result.Error($"Team member '{name}' already exists");
    }

    var teamMember = new TeamMember(name, userId);
    TeamMembers.Add(teamMember);

    return Result.Success();
  }
}
