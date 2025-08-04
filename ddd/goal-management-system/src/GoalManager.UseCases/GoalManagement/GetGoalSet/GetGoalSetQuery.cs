namespace GoalManager.UseCases.GoalManagement.GetGoalSet;

public record GetGoalSetQuery(int TeamId, int Year, int UserId) : IQuery<Result<GoalSetDto?>>;
