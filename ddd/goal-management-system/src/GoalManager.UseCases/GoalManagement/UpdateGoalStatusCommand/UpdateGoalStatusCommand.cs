using GoalManager.Core.GoalManagement;

namespace GoalManager.UseCases.GoalManagement.UpdateGoalStatusCommand;
public record UpdateGoalStatusCommand(
    int GoalSetId,
    int GoalId,
    GoalProgressStatus Status,
    string? Comment = null) : ICommand<Result>;
