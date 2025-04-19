namespace GoalManager.UseCases.GoalManagement.GetPendingApprovalGoals;
public record GetPendingApprovalGoalsQuery(int TeamLeaderUserId) : IQuery<List<PendingApprovalGoalDto>>;
