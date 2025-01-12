using GoalManager.Core.Organisation.Events;

namespace GoalManager.Core.Organisation;

public class Organisation : EntityBase, IAggregateRoot
{
  private Organisation(string name)
  {
    Name = Guard.Against.NullOrWhiteSpace(name);
  }

  private const int MaxTeamCount = 5;

  public string Name { get; private set; }

  public ICollection<Team> Teams { get; } = new List<Team>();

  internal void RegisterOrganisationCreatedEvent()
  {
    RegisterDomainEvent(new OrganisationCreatedEvent(Name));
  }

  internal static Result<Organisation> Create(string name)
  {
    if (string.IsNullOrWhiteSpace(name))
    {
      return Result<Organisation>.Error("Organisation name is required");
    }

    var organisation = new Organisation(name);
    organisation.RegisterOrganisationCreatedEvent();
    return organisation;
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
   
    var teamResult = Team.Create(name, Id);
    if (!teamResult.IsSuccess)
    {
      return teamResult.ToResult();
    }

    Teams.Add(teamResult);

    return Result.Success();
  }

  internal void Delete()
  {
    RegisterDomainEvent(new OrganisationDeletedEvent(Id, Name));
  }

  internal Result Update(string name)
  {
    var result = UpdateName(name);
    if (!result.IsSuccess)
    {
      return result;
    }

    RegisterDomainEvent(new OrganisationUpdatedEvent(Id, Name));

    return result;
  }

  private Result UpdateName(string name)
  {
    if (string.IsNullOrWhiteSpace(name))
    { 
      return Result.Error("Organisation name is required");
    }

    Name = name;
    return Result.Success();
  }
}
