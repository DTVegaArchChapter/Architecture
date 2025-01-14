using GoalManager.UseCases.Organisation.GetTeamForUpdate;
using GoalManager.UseCases.Organisation.List;

namespace GoalManager.UseCases.Organisation;

public interface IOrganisationQueryService
{
  Task<List<OrganisationListItemDto>> ListAsync(int? skip, int? take);

  Task<TeamForUpdateDto?> GetTeamForUpdate(int id);
}
