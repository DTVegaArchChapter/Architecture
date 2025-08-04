namespace GoalManager.UseCases.GoalManagement.CreateGoalSet;

public record CreateGoalSetCommand(int TeamId, int Year, int UserId) : ICommand<Result>;
