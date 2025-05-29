
using GoalManager.Core.GoalManagement;

namespace GoalManager.UseCases.GoalManagement.GetAllGoalSetsByTeamId;

public class GetAllGoalSetsByTeamIdQuery: ICommand<Result<List<GoalSet>>>
{
  public int TeamId { get; }
  public GetAllGoalSetsByTeamIdQuery(int teamId)
  {
    TeamId = teamId;
  }
}
