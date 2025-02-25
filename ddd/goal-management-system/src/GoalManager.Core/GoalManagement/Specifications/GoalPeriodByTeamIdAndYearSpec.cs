namespace GoalManager.Core.GoalManagement.Specifications;
public sealed class GoalPeriodByTeamIdAndYearSpec : Specification<GoalPeriod>
{
  public GoalPeriodByTeamIdAndYearSpec(int teamId, int year) => Query.Where(x => x.TeamId == teamId && x.Year == year);
}
