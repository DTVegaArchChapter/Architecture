namespace GoalManager.Core.OrganisationAggregate;

public class TeamMember(string name, int userId) : EntityBase
{
  public int UserId { get; private set; } = userId;

  public string Name { get; } = Guard.Against.NullOrWhiteSpace(name);
}
