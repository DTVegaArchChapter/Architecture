using GoalManager.Core.GoalManagement;

namespace GoalManager.UseCases.GoalManagement.GetGoal;
public record GetGoalQuery(int GoalSetId, int GoalId) : IQuery<Result<Goal>>;
