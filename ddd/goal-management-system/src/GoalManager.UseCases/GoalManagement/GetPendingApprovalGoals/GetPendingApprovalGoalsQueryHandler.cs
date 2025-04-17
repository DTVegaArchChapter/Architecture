namespace GoalManager.UseCases.GoalManagement.GetPendingApprovalGoals;
public sealed class GetPendingApprovalGoalsQueryHandler(IGoalManagementQueryService goalQueryService)
    : IQueryHandler<GetPendingApprovalGoalsQuery, List<PendingApprovalGoalDto>>
{
  public Task<List<PendingApprovalGoalDto>> Handle(GetPendingApprovalGoalsQuery request, CancellationToken cancellationToken)
  {
    return goalQueryService.GetPendingApprovalGoalsForTeamLeader(request.TeamLeaderUserId);
  }
}
