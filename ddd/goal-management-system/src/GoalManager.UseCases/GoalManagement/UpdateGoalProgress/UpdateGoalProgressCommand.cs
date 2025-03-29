namespace GoalManager.UseCases.GoalManagement.UpdateGoalProgress;

public record UpdateGoalProgressCommand(int GoalSetId, int GoalId, int ActualValue, string? Comment) : ICommand<Result<(int GoalSetId, int GoalId)>>;
