using GoalManager.UseCases.GoalManagement.GetPendingApprovalGoals;
using GoalManager.UseCases.GoalManagement.GetPendingLastApprovalGoalSets;

namespace GoalManager.UseCases.GoalManagement;

public interface IGoalManagementQueryService
{
  Task<List<PendingApprovalGoalDto>> GetPendingApprovalGoals(IList<int> teamIds);
  Task<List<GetPendingLastApprovalGoalSetsDto>> GetPendingLastApprovalGoalSets(IList<int> teamIds);
}
