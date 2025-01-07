namespace GoalManager.Core.OrganisationAggregate;

public class Organisation : EntityBase, IAggregateRoot
{
  private Organisation(string name)
  {
    Name = Guard.Against.NullOrWhiteSpace(name);
  }

  private const int MaxTeamCount = 5;

  public string Name { get; }

  public ICollection<Team> Teams { get; } = new List<Team>();

  public static Result<Organisation> Create(string name)
  {
    if (string.IsNullOrWhiteSpace(name))
    {
      return Result<Organisation>.Error("Organisation name is required");
    }

    return new Organisation(name);
  }

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
   
    var teamResult = Team.Create(name);
    if (!teamResult.IsSuccess)
    {
      return teamResult.ToResult();
    }

    Teams.Add(teamResult);

    return Result.Success();
  }
}
