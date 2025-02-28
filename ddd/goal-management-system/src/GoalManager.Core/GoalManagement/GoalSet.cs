namespace GoalManager.Core.GoalManagement;

public class GoalSet : EntityBase, IAggregateRoot
{
  private const int MaxGoalCount = 10;

#pragma warning disable CS8618 // Required by Entity Framework
  private GoalSet() { }
#pragma warning restore CS8618

  private GoalSet(int teamId, int periodId, int userId)
  {
    TeamId = teamId;
    PeriodId = periodId;
    UserId = userId;
  }

  private readonly IList<Goal> _goals = [];

  public int UserId { get; private set; }
  public int PeriodId { get; private set; }
  public int TeamId { get; private set; }

  public IReadOnlyCollection<Goal> Goals => _goals.AsReadOnly();

  public static Result<GoalSet> Create(int teamId, int periodId, int userId)
  {
    return new GoalSet(teamId, periodId, userId);
  }

  public Result AddGoal(string title, GoalType goalType, GoalValue goalValue, int percentage)
  {
    if (Goals.Count >= MaxGoalCount)
    {
      return Result.Error($"Goals count cannot be bigger than {MaxGoalCount}");
    }

    var createGoalResult = Goal.Create(Id, title, goalType, goalValue, percentage);
    if (!createGoalResult.IsSuccess)
    {
      return createGoalResult.ToResult();
    }

    var totalPercentage = GetGoalsTotalPercentage();
    if (totalPercentage + percentage > 100)
    {
      return Result.Error($"Total percentage of goals cannot exceed 100. Current total percentage is {totalPercentage}");
    }

    _goals.Add(createGoalResult.Value);

    return Result.Success();
  }

  private int GetGoalsTotalPercentage()
  {
    return _goals.Sum(x => x.Percentage);
  }
}
