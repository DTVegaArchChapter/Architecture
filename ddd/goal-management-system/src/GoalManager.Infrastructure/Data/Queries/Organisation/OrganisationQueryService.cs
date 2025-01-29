using GoalManager.UseCases.Organisation;
using GoalManager.UseCases.Organisation.GetTeamForUpdate;
using GoalManager.UseCases.Organisation.ListOrganisations;

namespace GoalManager.Infrastructure.Data.Queries.Organisation;

public sealed class OrganisationQueryService(AppDbContext dbContext) : IOrganisationQueryService
{
  public Task<List<OrganisationListItemDto>> ListAsync(int? skip, int? take)
  {
    return dbContext.Organisation.Select(x => new OrganisationListItemDto(x.Id, x.Name, x.Teams.Count))
      .Skip(skip ?? 0).Take(take ?? 10)
      .ToListAsync();
  }

  public Task<TeamForUpdateDto?> GetTeamForUpdate(int id)
  {
    return dbContext.Team.Where(x => x.Id == id).Select(
        x => new TeamForUpdateDto(x.Id, x.Name, x.TeamMembers.Select(y => new TeamMemberDto(y.Id, y.UserId, y.Name, y.MemberType.Name))))
      .SingleOrDefaultAsync();
  }

  public Task<string?> GetTeamNameAsync(int id)
  {
    return dbContext.Team.Where(x => x.Id == id).Select(x => x.Name).SingleOrDefaultAsync();
  }
}
