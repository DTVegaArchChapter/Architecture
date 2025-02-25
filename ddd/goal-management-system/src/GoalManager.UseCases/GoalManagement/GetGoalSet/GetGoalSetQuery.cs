using GoalManager.Core.GoalManagement;

namespace GoalManager.UseCases.GoalManagement.GetGoalSet;

public record GetGoalSetQuery(int TeamId, int Year, int UserId) : IQuery<Result<GoalSet>>;
