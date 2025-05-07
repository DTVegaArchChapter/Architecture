using GoalManager.Core.Organisation;
using GoalManager.Infrastructure.Data;
using GoalManager.UseCases.Organisation;
using GoalManager.UseCases.Organisation.GetTeamForUpdate;
using GoalManager.UseCases.Organisation.ListOrganisations;
using GoalManager.UseCases.Organisation.ListUserTeams;

namespace GoalManager.Infrastructure.Queries.Organisation;

public sealed class OrganisationQueryService(AppDbContext dbContext) : IOrganisationQueryService
{
  public Task<List<UserTeamListItemDto>> ListUserTeams(int userId)
  {
    return dbContext.TeamMember.AsNoTracking()
                  .Where(x => x.UserId == userId)
                  .Select(x => new UserTeamListItemDto
                               {
                                 TeamId = x.TeamId,
                                 TeamName = x.Team.Name.Value
                               }).ToListAsync();
  }

  public Task<List<OrganisationListItemDto>> ListAsync(int? skip, int? take)
  {
    return dbContext.Organisation.AsNoTracking().Select(x => new OrganisationListItemDto(x.Id, x.Name.Value, x.Teams.Count))
      .Skip(skip ?? 0).Take(take ?? 10)
      .ToListAsync();
  }

  public Task<TeamForUpdateDto?> GetTeamForUpdate(int id)
  {
    return dbContext.Team.AsNoTracking().Where(x => x.Id == id).Select(
        x => new TeamForUpdateDto(x.Id, x.Name.Value, x.TeamMembers.Select(y => new TeamMemberDto(y.Id, y.UserId, y.Name, y.MemberType.Name))))
      .SingleOrDefaultAsync();
  }

  public Task<string?> GetTeamNameAsync(int id)
  {
    return dbContext.Team.AsNoTracking().Where(x => x.Id == id).Select(x => x.Name.Value).SingleOrDefaultAsync();
  }

  public Task<List<int>> GetTeamLeaderUserIdsAsync(int teamId)
  {
    return dbContext.TeamMember.AsNoTracking().Where(x => x.TeamId == teamId && x.MemberType == TeamMemberType.TeamLeader).Select(x => x.UserId).ToListAsync();
  }

  public Task<List<int>> GetTeamIds(int teamLeaderUserId)
  {
      return dbContext.TeamMember
          .Where(tm => tm.UserId == teamLeaderUserId)
          .Select(x => x.TeamId)
          .ToListAsync();
  }

  public async Task<Dictionary<int, string>> GetTeamNamesAsync(List<int> teamIds)
  {
    return await dbContext.Team
        .Where(t => teamIds.Contains(t.Id))
        .ToDictionaryAsync(t => t.Id, t => t.Name.Value);
  }
}
