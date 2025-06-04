using GoalManager.Core.GoalManagement;
using GoalManager.UseCases.GoalManagement.CalculateGoalPointCommand;

namespace GoalManager.UseCases.PerformanceEvaluation.CalculatePerformanceEvaluation;

public record CalculatePerformanceEvaluationCommand(int TeamId) : ICommand<Result>;
