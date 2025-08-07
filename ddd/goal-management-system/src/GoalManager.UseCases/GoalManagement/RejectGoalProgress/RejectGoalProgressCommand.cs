namespace GoalManager.UseCases.GoalManagement.RejectGoalProgress;

public record RejectGoalProgressCommand(int GoalSetId, int GoalId) : ICommand<Result>;
