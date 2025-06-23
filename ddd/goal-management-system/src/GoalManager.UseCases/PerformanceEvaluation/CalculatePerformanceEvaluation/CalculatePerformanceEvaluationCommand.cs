namespace GoalManager.UseCases.PerformanceEvaluation.CalculatePerformanceEvaluation;

public record CalculatePerformanceEvaluationCommand(int TeamId) : ICommand<Result>;
