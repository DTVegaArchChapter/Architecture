namespace GoalManager.UseCases.GoalManagement.ApproveGoalSet;

public record ApproveGoalSetCommand(int GoalSetId) : ICommand<Result>;
