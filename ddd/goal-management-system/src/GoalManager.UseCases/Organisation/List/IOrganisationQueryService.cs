namespace GoalManager.UseCases.Organisation.List;

public interface IOrganisationQueryService
{
  Task<List<OrganisationListItemDto>> ListAsync(int? skip, int? take);
}
