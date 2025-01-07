namespace GoalManager.Core.OrganisationAggregate;

public class Team : EntityBase
{
  private Team(string name)
  {
    Name = Guard.Against.NullOrWhiteSpace(name);
  }

  private const int MaxTeamMemberCount = 10;

  public string Name { get; }

  public ICollection<TeamMember> TeamMembers { get; } = new List<TeamMember>();

  public static Result<Team> Create(string name)
  {
    if (string.IsNullOrWhiteSpace(name))
    {
      return Result<Team>.Error("Team name is required");
    }

    return new Team(name);
  }

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

    var teamMemberResult = TeamMember.Create(name, userId);
    if (!teamMemberResult.IsSuccess)
    {
      return teamMemberResult.ToResult();
    }

    TeamMembers.Add(teamMemberResult.Value);

    return Result.Success();
  }
}
