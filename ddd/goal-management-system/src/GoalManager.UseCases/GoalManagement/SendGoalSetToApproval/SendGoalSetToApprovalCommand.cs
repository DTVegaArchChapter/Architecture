namespace GoalManager.UseCases.GoalManagement.SendGoalSetToApproval;

public record SendGoalSetToApprovalCommand(int GoalSetId) : ICommand<Result>;
