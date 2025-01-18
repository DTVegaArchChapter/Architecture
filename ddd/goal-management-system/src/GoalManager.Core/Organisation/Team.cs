using GoalManager.Core.Organisation.Events;

namespace GoalManager.Core.Organisation;

public class Team : EntityBase
{
#pragma warning disable CS8618 // Required by Entity Framework
  private Team() { }
#pragma warning restore CS8618
 
  private Team(string name, int organisationId, ICollection<TeamMember> teamMembers)
  {
    OrganisationId = organisationId;
    SetName(name);
    TeamMembers = teamMembers;
  }

  private const int MaxTeamMemberCount = 10;

  public string Name { get; private set; } = null!;

  public int OrganisationId { get; private set; }

  public ICollection<TeamMember> TeamMembers { get; }

  public Organisation Organisation { get; private set; } = null!;

  internal static Result<Team> Create(string name, int organisationId)
  {
    if (string.IsNullOrWhiteSpace(name))
    {
      return Result<Team>.Error("Team name is required");
    }

    var team = new Team(name, organisationId, new List<TeamMember>());
    team.RegisterTeamCreatedEvent();
    return team;
  }

  internal Result AddTeamMember(string name, int userId)
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

  private void RegisterTeamCreatedEvent()
  {
    RegisterDomainEvent(new TeamCreatedEvent(Name));
  }

  public void Update(string name)
  {
    SetName(name);

    RegisterDomainEvent(new TeamUpdatedEvent(Id, Name));
  }

  private void SetName(string name)
  {
    Name = Guard.Against.NullOrWhiteSpace(name);
  }
}
