using GoalManager.UseCases.GoalManagement.GetPendingApprovalGoals;

namespace GoalManager.UseCases.GoalManagement;
public interface IGoalManagementQueryService
{
  Task<List<PendingApprovalGoalDto>> GetPendingApprovalGoalsForTeamLeader(int teamLeaderUserId);

}
