using GoalManager.Core.Organisation.Events;

namespace GoalManager.Core.Organisation;

public class Team : EntityBase
{
  private readonly IList<TeamMember> _teamMembers = new List<TeamMember>();

#pragma warning disable CS8618 // Required by Entity Framework
  private Team() { }
#pragma warning restore CS8618
 
  private Team(string name, int organisationId, IList<TeamMember> teamMembers)
  {
    OrganisationId = organisationId;
    SetName(name);
    _teamMembers = teamMembers;
  }

  private const int MaxTeamMemberCount = 10;

  public string Name { get; private set; } = null!;

  public int OrganisationId { get; private set; }

  public IReadOnlyCollection<TeamMember> TeamMembers => _teamMembers.AsReadOnly();

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

  internal Result AddTeamMember(string name, int userId, TeamMemberType memberType)
  {
    Guard.Against.NegativeOrZero(userId);
    Guard.Against.Null(memberType);

    if (TeamMembers.Count >= MaxTeamMemberCount)
    {
      return Result.Error($"Team member count cannot be bigger than {MaxTeamMemberCount}");
    }

    if (TeamMembers.Any(x => x.UserId == userId))
    {
      return Result.Error($"Team member '{name}' already exists");
    }

    if (memberType == TeamMemberType.TeamLeader && TeamMembers.Count(x => x.MemberType == TeamMemberType.TeamLeader) == 1)
    {
      return Result.Error("Only one team leader per team is allowed");
    }

    var teamMemberResult = TeamMember.Create(name, userId, Id, memberType);
    if (!teamMemberResult.IsSuccess)
    {
      return teamMemberResult.ToResult();
    }

    _teamMembers.Add(teamMemberResult.Value);

    return Result.Success();
  }

  internal Result RemoveTeamMember(int userId)
  {
    Guard.Against.NegativeOrZero(userId);

    var teamMember = _teamMembers.SingleOrDefault(x => x.UserId == userId);
    if (teamMember == null)
    {
      return Result.Error("Team member not found");
    }

    _teamMembers.Remove(teamMember);

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
