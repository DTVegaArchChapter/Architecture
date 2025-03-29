using GoalManager.Core.GoalManagement;

namespace GoalManager.UseCases.GoalManagement.UpdateGoalProgress;
public record UpdateGoalProgressCommand(int GoalSetId, int GoalId, int ActualValue, string? Comment, GoalProgressStatus GoalProgressStatus) : ICommand<Result<(int GoalSetId, int GoalId)>>;
