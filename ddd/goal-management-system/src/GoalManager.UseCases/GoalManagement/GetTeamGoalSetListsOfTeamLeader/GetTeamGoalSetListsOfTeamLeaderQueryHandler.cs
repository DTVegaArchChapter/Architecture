using GoalManager.UseCases.Identity;
using GoalManager.UseCases.Organisation;
using GoalManager.UseCases.PerformanceEvaluation;

namespace GoalManager.UseCases.GoalManagement.GetTeamGoalSetListsOfTeamLeader;

internal sealed class GetTeamGoalSetListsOfTeamLeaderQueryHandler(
  IGoalManagementQueryService goalQueryService,
  IOrganisationQueryService organisationQueryService,
  IPerformanceEvaluationQueryService performanceEvaluationQueryService,
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
    var goalSetEvaluationIds = await performanceEvaluationQueryService.GetRelatedGoalSetEvaluationsAsync(teamGoalSets.Select(x => x.GoalSetId).ToList(), cancellationToken).ConfigureAwait(false);

    return teamGoalSets.GroupBy(x => x.TeamId).Select(
      x => new TeamGoalSetListItem
      {
             TeamId = x.Key, 
             TeamName = teamNamesDict.TryGetValue(x.Key, out var v1) ? v1 : string.Empty,
             GoalSets = x.Select(y => new GoalSetListItemDto
                                      {
                                        Id = y.GoalSetId,
                                        GoalSetEvaluationId = goalSetEvaluationIds.Where(z => z.GoalSetId == y.GoalSetId).Select(z => (int?)z.GoalSetEvaluationId).FirstOrDefault(),
                                        User = userEmails.TryGetValue(y.UserId, out var v2) ? v2 : string.Empty,
                                        Status = y.Status
                                      }).ToList()
           }).ToList();
  }
}
