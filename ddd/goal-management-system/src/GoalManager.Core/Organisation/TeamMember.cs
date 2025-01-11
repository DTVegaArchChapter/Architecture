namespace GoalManager.Core.Organisation;

public class TeamMember : EntityBase
{
  private TeamMember(string name, int userId, int teamId)
  {
    UserId = Guard.Against.NegativeOrZero(userId);
    Name = Guard.Against.NullOrWhiteSpace(name);
    TeamId = teamId;
  }

  public int UserId { get; private set; }

  public string Name { get; }

  public int TeamId { get; private set; }

  public Team Team { get; private set; } = null!;

  public static Result<TeamMember> Create(string name, int userId, int teamId)
  {
    if (string.IsNullOrWhiteSpace(name))
    {
      return Result<TeamMember>.Error("Team member name is required");
    }

    return new TeamMember(name, userId, teamId);
  }
}
