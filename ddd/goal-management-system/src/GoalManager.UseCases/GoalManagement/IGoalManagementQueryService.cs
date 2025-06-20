using GoalManager.UseCases.GoalManagement.GetPendingApprovalGoals;
using GoalManager.UseCases.GoalManagement.GetPendingLastApprovalGoalSets;
using GoalManager.UseCases.GoalManagement.GetTeamPerformanceData;

namespace GoalManager.UseCases.GoalManagement;

public interface IGoalManagementQueryService
{
  Task<List<PendingApprovalGoalDto>> GetPendingApprovalGoals(IList<int> teamIds);

  Task<List<GetPendingLastApprovalGoalSetsDto>> GetPendingLastApprovalGoalSets(IList<int> teamIds);

  Task<TeamPerformanceDataDto> GetTeamPerformanceData(int teamId);

  Task<List<TeamMemberGoalSetListItemDto>> GetTeamMemberGoalSetsList(IList<int> teamIds);
}
