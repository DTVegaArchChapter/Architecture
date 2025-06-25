namespace GoalManager.UseCases.PerformanceEvaluation.GetPerformanceEvaluationReport;

public record GetPerformanceEvaluationReportQuery(int GoalSetId) : IQuery<GoalSetEvaluationDto?>;
