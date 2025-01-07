namespace GoalManager.Core.OrganisationAggregate;

public class Organisation(string name) : EntityBase, IAggregateRoot
{
  private const int MaxTeamCount = 5;

  public string Name { get; } = Guard.Against.NullOrWhiteSpace(name);

  public ICollection<Team> Teams { get; } = new List<Team>();

  public Result AddTeam(string name)
  {
    if (Teams.Count >= MaxTeamCount)
    {
      return Result.Error($"Teams count cannot be bigger than {MaxTeamCount}");
    }

    if (Teams.Any(x => string.Equals(x.Name, name, StringComparison.CurrentCultureIgnoreCase)))
    {
      return Result.Error($"Team with name '{name}' already exists");
    }
   
    var team = new Team(name);
    Teams.Add(team);

    return Result.Success();
  }
}
