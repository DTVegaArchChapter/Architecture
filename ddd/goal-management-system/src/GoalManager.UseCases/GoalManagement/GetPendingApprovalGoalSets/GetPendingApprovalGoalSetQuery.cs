namespace GoalManager.UseCases.GoalManagement.GetPendingApprovalGoalSets;
public record GetPendingApprovalGoalSetsQuery(int TeamLeaderUserId) : IQuery<List<PendingApprovalGoalSetDto>>;
