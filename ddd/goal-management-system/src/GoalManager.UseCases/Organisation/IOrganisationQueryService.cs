using GoalManager.UseCases.Organisation.GetTeamForUpdate;
using GoalManager.UseCases.Organisation.ListOrganisations;

namespace GoalManager.UseCases.Organisation;

public interface IOrganisationQueryService
{
  Task<List<OrganisationListItemDto>> ListAsync(int? skip, int? take);

  Task<TeamForUpdateDto?> GetTeamForUpdate(int id);

  Task<string?> GetTeamNameAsync(int id);
}
