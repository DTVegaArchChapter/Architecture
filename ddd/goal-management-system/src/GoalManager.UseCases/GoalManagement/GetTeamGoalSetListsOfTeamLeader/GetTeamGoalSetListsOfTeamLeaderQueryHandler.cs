using GoalManager.UseCases.Identity;
using GoalManager.UseCases.Organisation;

namespace GoalManager.UseCases.GoalManagement.GetTeamGoalSetListsOfTeamLeader;

internal sealed class GetTeamGoalSetListsOfTeamLeaderQueryHandler(
  IGoalManagementQueryService goalQueryService,
  IOrganisationQueryService organisationQueryService,
  IIdentityQueryService identityQueryService)
  : IQueryHandler<GetTeamGoalSetListsOfTeamLeaderQuery, Result<List<TeamGoalSetListItem>>>
{
  public async Task<Result<List<TeamGoalSetListItem>>> Handle(GetTeamGoalSetListsOfTeamLeaderQuery request, CancellationToken cancellationToken)
  {
    var teamLeaderTeams = await organisationQueryService.ListTeamLeaderTeams(request.TeamLeaderUserId).ConfigureAwait(false);
    var teamIds = teamLeaderTeams.Select(x => x.TeamId).ToList();
    var teamGoalSets = await goalQueryService.GetTeamMemberGoalSetsList(teamIds).ConfigureAwait(false);
    var teamNamesDict = await organisationQueryService.GetTeamNamesAsync(teamIds).ConfigureAwait(false);
    var userEmails = await identityQueryService.GetUserEmails(teamGoalSets.Select(x => x.UserId).Distinct().ToList()).ConfigureAwait(false);

    return teamGoalSets.GroupBy(x => x.TeamId).Select(
      x => new TeamGoalSetListItem
      {
             TeamId = x.Key, 
             TeamName = teamNamesDict.TryGetValue(x.Key, out var v1) ? v1 : string.Empty,
             GoalSets = x.Select(y => new GoalSetListItemDto
                                      {
                                        User = userEmails.TryGetValue(y.UserId, out var v2) ? v2 : string.Empty,
                                        Status = y.Status
                                      }).ToList()
           }).ToList();
  }
}
