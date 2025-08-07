namespace GoalManager.UseCases.GoalManagement.ApproveGoalProgress;

public record ApproveGoalProgressCommand(int GoalSetId, int GoalId) : ICommand<Result>;
