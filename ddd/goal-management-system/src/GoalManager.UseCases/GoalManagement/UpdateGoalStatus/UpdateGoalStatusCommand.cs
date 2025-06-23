using GoalManager.Core.GoalManagement;

namespace GoalManager.UseCases.GoalManagement.UpdateGoalStatus;

public record UpdateGoalStatusCommand(
    int GoalSetId,
    int GoalId,
    GoalProgressStatus Status,
    string? Comment = null) : ICommand<Result>;
