namespace GoalManager.UseCases.GoalManagement.RejectGoalSet;

public record RejectGoalSetCommand(int GoalSetId) : ICommand<Result>;
