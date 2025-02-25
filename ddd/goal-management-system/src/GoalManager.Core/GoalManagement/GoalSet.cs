namespace GoalManager.Core.GoalManagement;
public class GoalSet : EntityBase, IAggregateRoot
{
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

  public Result AddGoal(string title, GoalType goalType, GoalValue goalValue)
  {
    var createGoalResult = Goal.Create(Id, title, goalType, goalValue);
    if (!createGoalResult.IsSuccess)
    {
      return createGoalResult.ToResult();
    }

    return Result.Success();
  }
}
