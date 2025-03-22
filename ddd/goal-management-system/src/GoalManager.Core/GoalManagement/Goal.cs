using GoalManager.Core.GoalManagement.Events;

namespace GoalManager.Core.GoalManagement;

public class Goal : EntityBase
{
  public string Title { get; private set; }
  public GoalType GoalType { get; private set; }
  public GoalValue GoalValue { get; private set; }
  public int? ActualValue { get; private set; }
  public int GoalSetId { get; private set; }
  public int Percentage { get; private set; }

  private readonly IList<GoalProgress> _goalProgressHistory = [];
  public IReadOnlyCollection<GoalProgress> GoalProgressHistory => _goalProgressHistory.AsReadOnly();

#pragma warning disable CS8618 // Required by Entity Framework
  private Goal() { }
#pragma warning restore CS8618

  private Goal(int goalSetId, string title, GoalType goalType, GoalValue goalValue, int percentage)
  {
    Title = Guard.Against.NullOrWhiteSpace(title);
    GoalType = goalType;
    GoalValue = goalValue;
    GoalSetId = goalSetId;
    Percentage = percentage;
  }

  public void SetActualValue(int value)
  {
    ActualValue = value;
  }

  internal static Result<Goal> Create(int goalSetId, string title, GoalType goalType, GoalValue goalValue, int percentage)
  {
    if (string.IsNullOrWhiteSpace(title))
    {
      return Result<Goal>.Error("Goal title is required");
    }

    var goal = new Goal(goalSetId, title, goalType, goalValue, percentage);
    goal.RegisterGoalCreatedEvent();
    return goal;
  }

  internal Result Update(string title, GoalType goalType, GoalValue goalValue, int percantage)
  {
    if (string.IsNullOrWhiteSpace(title))
    {
      return Result.Error("Goal title is required");
    }

    Title = title;
    GoalType = goalType;
    GoalValue = goalValue;
    Percentage = percantage;

    return Result.Success();
  }

  private void RegisterGoalCreatedEvent()
  {
    RegisterDomainEvent(new GoalCreatedEvent(Title));
  }
}
