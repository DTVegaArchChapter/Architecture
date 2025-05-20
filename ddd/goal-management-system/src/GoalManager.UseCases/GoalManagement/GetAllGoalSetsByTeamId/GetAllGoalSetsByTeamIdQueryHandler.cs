
using GoalManager.Core.GoalManagement;
using GoalManager.Core.GoalManagement.Specifications;

namespace GoalManager.UseCases.GoalManagement.GetAllGoalSetsByTeamId;
public class GetAllGoalSetsByTeamIdQueryHandler(IRepository<GoalSet> goalSetRepository) : ICommandHandler<GetAllGoalSetsByTeamIdQuery, Result<List<GoalSet>>>
{
  public async Task<Result<List<GoalSet>>> Handle(GetAllGoalSetsByTeamIdQuery request, CancellationToken cancellationToken)
  {
    var goalSets = await goalSetRepository.ListAsync(new GoalSetsWithGoalsByTeamIdSpec(request.TeamId));

    if (goalSets == null)
    {
      return Result.Error($"GoalSets not found");
    }

    return goalSets;

  }
}
