namespace GoalManager.Core.GoalManagement.Specifications;

public sealed class GoalSetWithGoalsByPeriodIdTeamIdUserIdSpec : SingleResultSpecification<GoalSet>
{
  public GoalSetWithGoalsByPeriodIdTeamIdUserIdSpec(int periodId, int teamId, int userId) => Query.Include(x => x.Goals).ThenInclude(x => x.GoalProgressHistory).Where(x => x.PeriodId == periodId && x.TeamId == teamId && x.UserId == userId);
}
