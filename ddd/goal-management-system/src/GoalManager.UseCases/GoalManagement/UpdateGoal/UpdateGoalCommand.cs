using GoalManager.Core.GoalManagement;

namespace GoalManager.UseCases.GoalManagement.UpdateGoal;
public record UpdateGoalCommand(int GoalSetId, int GoalId, string Title, GoalType GoalType) : ICommand<Result>;
