namespace GoalManager.UseCases.PerformanceEvaluation.GetPerformanceEvaluationReport;

public sealed class GoalSetEvaluationDto
{
  private List<GoalEvaluationDto>? _goalEvaluations;

  public int Id { get; set; }

  public int TeamId { get; set; }

  public string? TeamName { get; set; }

  public int UserId { get; set; }

  public string? User { get; set; }

  public int Year { get; set; }

  public int GoalSetId { get; set; }

  public double? PerformanceScore { get; set; }

  public string? PerformanceGrade { get; set; }

  public List<GoalEvaluationDto> GoalEvaluations
  {
    get => _goalEvaluations ??= [];
    set => _goalEvaluations = value;
  }
}
