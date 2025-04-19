using GoalManager.UseCases.Organisation.GetTeamForUpdate;
using GoalManager.UseCases.Organisation.ListOrganisations;
using GoalManager.UseCases.Organisation.ListUserTeams;

namespace GoalManager.UseCases.Organisation;

public interface IOrganisationQueryService
{
  Task<List<OrganisationListItemDto>> ListAsync(int? skip, int? take);

  Task<TeamForUpdateDto?> GetTeamForUpdate(int id);

  Task<string?> GetTeamNameAsync(int id);

  Task<List<UserTeamListItemDto>> ListUserTeams(int userId);
  Task<Dictionary<int, List<int>>> GetTeamMemberUserIdsByTeamLeader(int teamLeaderUserId);
  Task<Dictionary<int, string>> GetTeamNamesAsync(List<int> teamIds);

}
