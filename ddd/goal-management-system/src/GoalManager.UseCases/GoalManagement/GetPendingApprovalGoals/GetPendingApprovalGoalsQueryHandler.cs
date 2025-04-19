using GoalManager.UseCases.Organisation;

namespace GoalManager.UseCases.GoalManagement.GetPendingApprovalGoals;
public sealed class GetPendingApprovalGoalsQueryHandler(IGoalManagementQueryService goalQueryService, IOrganisationQueryService organisationQueryService)
    : IQueryHandler<GetPendingApprovalGoalsQuery, List<PendingApprovalGoalDto>>
{
  public async Task<List<PendingApprovalGoalDto>> Handle(GetPendingApprovalGoalsQuery request, CancellationToken cancellationToken)
  {
    var teamMembers = await organisationQueryService.GetTeamMemberUserIdsByTeamLeader(request.TeamLeaderUserId);
    var teamIds = teamMembers.SelectMany(x => x.Value).Distinct().ToList();
    var teamNamesDict = await organisationQueryService.GetTeamNamesAsync(teamIds);

    return await goalQueryService.GetPendingApprovalGoalsForTeamLeader(request.TeamLeaderUserId, teamMembers, teamNamesDict);
  }
}
