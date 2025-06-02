namespace GoalManager.Core.PerformanceEvaluation;

internal class GoalSetEvaluation : EntityBase, IAggregateRoot
{
  private readonly IList<GoalEvaluation> _goalEvaluations = [];

  public int GoalSetId { get; private set; }

  public double? Point { get; private set; }

  public string? CharacterPoint { get; private set; }

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

  public Result SetCharacterPoint(string character)
  {
    CharacterPoint = character;
    return Result.Success();
  }

  public Result CalculateAllGoalPoint()
  {
    foreach (var goal in _goalEvaluations)
    {
      goal.CalculatePoint();
    }

    Point = _goalEvaluations.Sum(x => x.Point.GetValueOrDefault() * (x.Percentage / 100.0));

    return Result.Success();
  }
}
