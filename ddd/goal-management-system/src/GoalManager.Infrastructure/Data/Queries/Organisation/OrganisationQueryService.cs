﻿using GoalManager.UseCases.Organisation;
using GoalManager.UseCases.Organisation.GetTeamForUpdate;
using GoalManager.UseCases.Organisation.ListOrganisations;
using GoalManager.UseCases.Organisation.ListUserTeams;

namespace GoalManager.Infrastructure.Data.Queries.Organisation;

public sealed class OrganisationQueryService(AppDbContext dbContext) : IOrganisationQueryService
{
  public Task<List<UserTeamListItemDto>> ListUserTeams(int userId)
  {
    return dbContext.TeamMember
                  .Where(x => x.UserId == userId)
                  .Select(x => new UserTeamListItemDto
                               {
                                 TeamId = x.TeamId,
                                 TeamName = x.Team.Name.Value
                               }).ToListAsync();
  }

  public Task<List<OrganisationListItemDto>> ListAsync(int? skip, int? take)
  {
    return dbContext.Organisation.Select(x => new OrganisationListItemDto(x.Id, x.Name.Value, x.Teams.Count))
      .Skip(skip ?? 0).Take(take ?? 10)
      .ToListAsync();
  }

  public Task<TeamForUpdateDto?> GetTeamForUpdate(int id)
  {
    return dbContext.Team.Where(x => x.Id == id).Select(
        x => new TeamForUpdateDto(x.Id, x.Name.Value, x.TeamMembers.Select(y => new TeamMemberDto(y.Id, y.UserId, y.Name, y.MemberType.Name))))
      .SingleOrDefaultAsync();
  }

  public Task<string?> GetTeamNameAsync(int id)
  {
    return dbContext.Team.Where(x => x.Id == id).Select(x => x.Name.Value).SingleOrDefaultAsync();
  }
}
