using GoalManager.Core.GoalManagement;

namespace GoalManager.UseCases.GoalManagement.UpdateGoalSetStatus;

public record UpdateGoalSetStatusCommand(int GoalSetId, GoalSetStatus? Status) : ICommand<Result>;
