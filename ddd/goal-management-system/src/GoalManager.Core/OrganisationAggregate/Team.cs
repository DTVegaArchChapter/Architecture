namespace GoalManager.Core.OrganisationAggregate;

public class Team : EntityBase
{
  private Team(string name, int organisationId)
  {
    OrganisationId = organisationId;
    Name = Guard.Against.NullOrWhiteSpace(name);
  }

  private const int MaxTeamMemberCount = 10;

  public string Name { get; }

  public int OrganisationId { get; private set; }

  public ICollection<TeamMember> TeamMembers { get; } = new List<TeamMember>();

  public Organisation Organisation { get; private set; } = null!;

  public static Result<Team> Create(string name, int organisationId)
  {
    if (string.IsNullOrWhiteSpace(name))
    {
      return Result<Team>.Error("Team name is required");
    }

    return new Team(name, organisationId);
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

    var teamMemberResult = TeamMember.Create(name, userId, Id);
    if (!teamMemberResult.IsSuccess)
    {
      return teamMemberResult.ToResult();
    }

    TeamMembers.Add(teamMemberResult.Value);

    return Result.Success();
  }
}
