namespace GoalManager.UseCases.GoalManagement.ApproveGoalSet;

public record ApproveGoalSetCommand(int GoalSetId, int UserId) : ICommand<Result>;
