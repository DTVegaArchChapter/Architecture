
using GoalManager.Core.Organisation;
using GoalManager.UseCases.Identity;
using GoalManager.UseCases.Organisation;

namespace GoalManager.UseCases.GoalManagement.GetPendingLastApprovalGoalSets;
public class GetPendingLastApprovalGoalSetsQuery: IQuery<Result<List<GetPendingLastApprovalGoalSetsDto>>>
{
  public int  TeamLeaderUserId { get; set; }

  public GetPendingLastApprovalGoalSetsQuery(int teamLeaderUserId)
  {
    TeamLeaderUserId = teamLeaderUserId;
  }

  public class GetPendingLastApprovalGoalSetsQueryHandler(IGoalManagementQueryService goalQueryService, IOrganisationQueryService organisationQueryService, IIdentityQueryService identityQueryService) : IQueryHandler<GetPendingLastApprovalGoalSetsQuery, Result<List<GetPendingLastApprovalGoalSetsDto>>>
  {
    public async Task<Result<List<GetPendingLastApprovalGoalSetsDto>>> Handle(GetPendingLastApprovalGoalSetsQuery request, CancellationToken cancellationToken)
    {

      var userTeams = await organisationQueryService.ListUserTeams(request.TeamLeaderUserId).ConfigureAwait(false);
      var teamIds = userTeams.Where(x => x.TeamMemberType == TeamMemberType.TeamLeader).Select(x => x.TeamId).ToList();

      if(teamIds == null || !teamIds.Any())
        return Result.Success(new List<GetPendingLastApprovalGoalSetsDto>());

      var pendingGoalSets = await goalQueryService.GetPendingLastApprovalGoalSets(teamIds!).ConfigureAwait(false);
      var teamNamesDict = await organisationQueryService.GetTeamNamesAsync(teamIds!).ConfigureAwait(false);
      var userEmails = await identityQueryService.GetUserEmails(pendingGoalSets.Select(x => x.UserId).Distinct().ToList()).ConfigureAwait(false);

      pendingGoalSets.ForEach(
         x =>
         {
           x.TeamName = teamNamesDict.TryGetValue(x.TeamId, out var v1) ? v1 : string.Empty;
           x.User = userEmails.TryGetValue(x.UserId, out var v2) ? v2 : string.Empty;
         });

      return Result.Success(pendingGoalSets);
    }
  }
}
