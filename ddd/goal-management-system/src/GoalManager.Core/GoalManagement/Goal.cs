using GoalManager.Core.GoalManagement.Events;

namespace GoalManager.Core.GoalManagement;

public class Goal : EntityBase
{
  public string Title { get; private set; }
  public GoalType GoalType { get; private set; }
  public GoalValue GoalValue { get; private set; }
  public int? ActualValue { get; private set; }
  public int PeriodId { get; private set; }


  private readonly IList<GoalProgress> _goalProgressHistory = [];
  public IReadOnlyCollection<GoalProgress> GoalProgressHistory => _goalProgressHistory.AsReadOnly();

#pragma warning disable CS8618 // Required by Entity Framework
  private Goal() { }
#pragma warning restore CS8618

  private Goal(int periodId, string title, GoalType goalType, GoalValue goalValue)
  {
    Title = Guard.Against.NullOrWhiteSpace(title);
    GoalType = goalType;
    GoalValue = goalValue;
    PeriodId = periodId;
  }

  public static Result<Goal> CreateTeamGoal(int goalPeriodId, string title, GoalValue goalValue)
  {
    return Create(goalPeriodId, title, GoalType.Team, goalValue);
  }

  public static Result<Goal> CreateIndividualGoal(int goalPeriodId, string title, GoalValue goalValue)
  {
    return Create(goalPeriodId, title, GoalType.Individual, goalValue);
  }

  public void SetActualValue(int value)
  {
    ActualValue = value;
  }

  private static Result<Goal> Create(int goalPeriodId, string title, GoalType goalType, GoalValue goalValue)
  {
    if (string.IsNullOrWhiteSpace(title))
    {
      return Result<Goal>.Error("Goal title is required");
    }

    var goal = new Goal(goalPeriodId, title, goalType, goalValue);
    goal.RegisterGoalCreatedEvent();
    return goal;
  }

  private void RegisterGoalCreatedEvent()
  {
    RegisterDomainEvent(new GoalCreatedEvent(Title));
  }
}
