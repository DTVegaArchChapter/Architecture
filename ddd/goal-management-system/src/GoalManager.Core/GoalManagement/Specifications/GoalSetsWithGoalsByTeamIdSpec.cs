namespace GoalManager.Core.GoalManagement.Specifications;
public sealed class GoalSetsWithGoalsByTeamIdSpec: Specification<GoalSet>
{
  public GoalSetsWithGoalsByTeamIdSpec(int teamId)
  
    => Query.Include(x => x.Goals)
        .ThenInclude(x => x.GoalProgressHistory)
        .Where(x => x.TeamId == teamId);
  
}
