namespace GoalManager.UseCases.GoalManagement.RejectGoalSet;

public record RejectGoalSetCommand(int GoalSetId, int UserId) : ICommand<Result>;
