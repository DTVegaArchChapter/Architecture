using GoalManager.UseCases.Organisation.List;

namespace GoalManager.Infrastructure.Data.Queries.Organisation;

public class OrganisationQueryService(AppDbContext dbContext) : IOrganisationQueryService
{
  public Task<List<OrganisationListItemDto>> ListAsync(int? skip, int? take)
  {
    return dbContext.Organisation.Select(x => new OrganisationListItemDto(x.Id, x.Name, x.Teams.Count))
      .Skip(skip ?? 0).Take(take ?? 10)
      .ToListAsync();
  }
}
