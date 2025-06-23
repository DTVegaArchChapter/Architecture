using GoalManager.UseCases.Identity;
using GoalManager.UseCases.Organisation;

namespace GoalManager.UseCases.GoalManagement.GetPendingApprovalGoals;
public sealed class GetPendingApprovalGoalsQueryHandler(IGoalManagementQueryService goalQueryService, IOrganisationQueryService organisationQueryService, IIdentityQueryService identityQueryService)
    : IQueryHandler<GetPendingApprovalGoalsQuery, List<PendingApprovalGoalDto>>
{
  public async Task<List<PendingApprovalGoalDto>> Handle(GetPendingApprovalGoalsQuery request, CancellationToken cancellationToken)
  {
    var teamIds = await organisationQueryService.GetTeamLeaderTeamIds(request.TeamLeaderUserId).ConfigureAwait(false);
    var pendingGoals = await goalQueryService.GetPendingApprovalGoals(teamIds).ConfigureAwait(false);
    var teamNamesDict = await organisationQueryService.GetTeamNamesAsync(teamIds).ConfigureAwait(false);
    var userEmails = await identityQueryService.GetUserEmails(pendingGoals.Select(x => x.UserId).Distinct().ToList()).ConfigureAwait(false);

    pendingGoals.ForEach(
      x =>
      {
        x.TeamName = teamNamesDict.TryGetValue(x.TeamId, out var v1) ? v1 : string.Empty;
        x.User = userEmails.TryGetValue(x.UserId, out var v2) ? v2 : string.Empty;
      });

    return pendingGoals;
  }
}
