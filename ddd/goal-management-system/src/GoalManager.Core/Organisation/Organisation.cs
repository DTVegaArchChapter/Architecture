using GoalManager.Core.Organisation.Events;

namespace GoalManager.Core.Organisation;

public class Organisation : EntityBase, IAggregateRoot
{
  private readonly IList<Team> _teams = new List<Team>();

#pragma warning disable CS8618 // Required by Entity Framework
  private Organisation() { }
  #pragma warning restore CS8618

  private Organisation(string name, IList<Team> teams)
  {
    Name = Guard.Against.NullOrWhiteSpace(name);
    _teams = teams;
  }

  private const int MaxTeamCount = 5;

  public string Name { get; private set; }

  public IReadOnlyCollection<Team> Teams => _teams.AsReadOnly();

  internal static Result<Organisation> Create(string name)
  {
    if (string.IsNullOrWhiteSpace(name))
    {
      return Result<Organisation>.Error("Organisation name is required");
    }

    var organisation = new Organisation(name, new List<Team>());
    organisation.RegisterOrganisationCreatedEvent();
    return organisation;
  }

  internal Result<Team> FindTeam(int teamId)
  {
    var team = Teams.SingleOrDefault(x => x.Id == teamId);
    if (team == null)
    {
      return Result<Team>.Error("Team not found");
    }

    return team;
  }

  internal Result AddTeam(string name)
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

    _teams.Add(teamResult);

    return Result.Success();
  }

  internal Result UpdateTeam(int teamId, string name)
  {
    if (Teams.Any(x => string.Equals(x.Name, name, StringComparison.CurrentCultureIgnoreCase)))
    {
      return Result.Error($"Team with name '{name}' already exists");
    }

    var teamResult = FindTeam(teamId);
    if (!teamResult.IsSuccess)
    {
      return teamResult.ToResult();
    }

    var team = teamResult.Value;
    team.Update(name);

    return Result.SuccessWithMessage("Team is updated");
  }

  internal Result DeleteTeam(int teamId)
  {
    var teamResult = FindTeam(teamId);
    if (!teamResult.IsSuccess)
    {
      return teamResult.ToResult();
    }

    var team = teamResult.Value;
    if (team.TeamMembers.Any())
    {
      return Result.Error("You cannot delete the team because there are team members added to the team");
    }

    _teams.Remove(team);

    RegisterDomainEvent(new TeamDeletedEvent(team.Id, team.Name));

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

  internal Result AddTeamMember(int teamId, string name, int userId)
  {
    var teamResult = FindTeam(teamId);
    if (!teamResult.IsSuccess)
    {
      return teamResult.ToResult();
    }

    var team = teamResult.Value;
    var addTeamMemberResult = team.AddTeamMember(name, userId);
    if (!addTeamMemberResult.IsSuccess)
    {
      return addTeamMemberResult.ToResult();
    }

    return Result.SuccessWithMessage("Team member is added");
  }

  private void RegisterOrganisationCreatedEvent()
  {
    RegisterDomainEvent(new OrganisationCreatedEvent(Name));
  }
}
