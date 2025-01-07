namespace GoalManager.Core.OrganisationAggregate;

public class TeamMember : EntityBase
{
  private TeamMember(string name, int userId)
  {
    UserId = Guard.Against.NegativeOrZero(userId);
    Name = Guard.Against.NullOrWhiteSpace(name);
  }

  public int UserId { get; private set; }

  public string Name { get; }

  public static Result<TeamMember> Create(string name, int userId)
  {
    if (string.IsNullOrWhiteSpace(name))
    {
      return Result<TeamMember>.Error("Team member name is required");
    }

    return new TeamMember(name, userId);
  }
}
