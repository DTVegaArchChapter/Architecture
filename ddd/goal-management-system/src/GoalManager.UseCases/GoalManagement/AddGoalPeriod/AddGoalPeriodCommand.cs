namespace GoalManager.UseCases.GoalManagement.AddGoalPeriod;

public record AddGoalPeriodCommand(int TeamId, int UserId, int Year) : ICommand<Result<int>>;
