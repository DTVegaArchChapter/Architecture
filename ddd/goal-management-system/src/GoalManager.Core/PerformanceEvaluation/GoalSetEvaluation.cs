namespace GoalManager.Core.PerformanceEvaluation;

public class GoalSetEvaluation : EntityBase, IAggregateRoot
{
  private readonly IList<GoalEvaluation> _goalEvaluations = [];

  public int GoalSetId { get; private set; }

  public double? PerformanceScore { get; private set; }

  public string? PerformanceGrade { get; private set; }

  public IReadOnlyCollection<GoalEvaluation> GoalEvaluations => _goalEvaluations.AsReadOnly();

#pragma warning disable CS8618 // Required by Entity Framework
  private GoalSetEvaluation() { }
#pragma warning restore CS8618

  private GoalSetEvaluation(int goalSetId, IList<GoalEvaluation> goalEvaluations)
  {
    _goalEvaluations = goalEvaluations ?? throw new ArgumentNullException(nameof(goalEvaluations));

    GoalSetId = goalSetId;
  }

  public static Result<GoalSetEvaluation> Create(int goalSetId, IList<GoalEvaluation> goalEvaluations)
  {
    return new GoalSetEvaluation(goalSetId, goalEvaluations);
  }

  public Result SetPerformanceGrade(string grade)
  {
    PerformanceGrade = grade;
    return Result.Success();
  }

  public Result CalculatePerformancePoint()
  {
    foreach (var goal in _goalEvaluations)
    {
      goal.CalculatePoint();
    }

    PerformanceScore = _goalEvaluations.Sum(x => x.Point.GetValueOrDefault() * (x.Percentage / 100.0));

    return Result.Success();
  }
}
