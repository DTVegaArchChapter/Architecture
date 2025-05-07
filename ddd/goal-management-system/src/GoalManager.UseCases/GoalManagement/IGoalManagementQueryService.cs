using GoalManager.UseCases.GoalManagement.GetPendingApprovalGoals;

namespace GoalManager.UseCases.GoalManagement;

public interface IGoalManagementQueryService
{
  Task<List<PendingApprovalGoalDto>> GetPendingApprovalGoals(IList<int> teamIds);
}
