using GoalManager.Core.GoalManagement.Events;

namespace GoalManager.Core.GoalManagement;

public class GoalPeriod : EntityBase, IAggregateRoot
{
  public int TeamId { get; private set; }
  public int Year { get; private set; }

#pragma warning disable CS8618 // Required by Entity Framework
  private GoalPeriod() { }
#pragma warning restore CS8618

  private GoalPeriod(int teamId, int year)
  {
    TeamId = teamId;
    Year = year;
  }

  public static Result<GoalPeriod> Create(int teamId, int year)
  {
    var goalPeriod = new GoalPeriod(teamId, year);
    goalPeriod.RegisterGoalPeriodCreatedEvent();
    return goalPeriod;
  }

  private void RegisterGoalPeriodCreatedEvent()
  {
    RegisterDomainEvent(new GoalPeriodCreatedEvent(TeamId, Year));
  }
}
