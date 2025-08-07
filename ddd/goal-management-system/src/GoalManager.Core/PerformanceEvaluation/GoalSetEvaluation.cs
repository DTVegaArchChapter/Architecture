using GoalManager.Core.PerformanceEvaluation.Events;

namespace GoalManager.Core.PerformanceEvaluation;

public class GoalSetEvaluation : EntityBase, IAggregateRoot
{
  private readonly IList<GoalEvaluation> _goalEvaluations = [];

  public int GoalSetId { get; private set; }

  public int Year { get; private set; }

  public int UserId { get; private set; }

  public int TeamId { get; private set; }

  public double? PerformanceScore { get; private set; }

  public string? PerformanceGrade { get; private set; }

  public IReadOnlyCollection<GoalEvaluation> GoalEvaluations => _goalEvaluations.AsReadOnly();

#pragma warning disable CS8618 // Required by Entity Framework
  private GoalSetEvaluation() { }
#pragma warning restore CS8618

  private GoalSetEvaluation(int goalSetId, int year, int userId, int teamId, IList<GoalEvaluation> goalEvaluations)
  {
    _goalEvaluations = goalEvaluations ?? throw new ArgumentNullException(nameof(goalEvaluations));

    GoalSetId = goalSetId;
    Year = year;
    UserId = userId;
    TeamId = teamId;
  }

  public static Result<GoalSetEvaluation> Create(int goalSetId, int year, int userId, int teamId, IList<GoalEvaluation> goalEvaluations)
  {
    var goalSetEvaluation = new GoalSetEvaluation(goalSetId, year, userId, teamId, goalEvaluations);
    var result = goalSetEvaluation.CalculatePerformanceScore();
    if (!result.IsSuccess)
    {
      return result;
    }

    goalSetEvaluation.RegisterDomainEvent(new GoalSetEvaluationCreatedEvent(goalSetId, year, userId, teamId));
    return goalSetEvaluation;
  }

  public Result SetPerformanceGrade(string grade)
  {
    PerformanceGrade = grade;
    return Result.Success();
  }

  private Result CalculatePerformanceScore()
  {
    foreach (var goal in _goalEvaluations)
    {
      goal.CalculatePoint();
    }

    PerformanceScore = _goalEvaluations.Sum(x => x.Point.GetValueOrDefault() * (x.Percentage / 100.0));

    return Result.Success();
  }
}
