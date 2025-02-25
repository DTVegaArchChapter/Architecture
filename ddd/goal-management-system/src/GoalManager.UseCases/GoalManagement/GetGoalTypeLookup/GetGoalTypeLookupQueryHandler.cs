using GoalManager.Core.GoalManagement;

namespace GoalManager.UseCases.GoalManagement.GetGoalTypeLookup;

internal sealed class GetGoalTypeLookupQueryHandler : IQueryHandler<GetGoalTypeLookupQuery, Result<List<GoalTypeLookupDto>>>
{
  public Task<Result<List<GoalTypeLookupDto>>> Handle(GetGoalTypeLookupQuery request, CancellationToken cancellationToken)
  {
    return Task.FromResult(
      new Result<List<GoalTypeLookupDto>>(
        GoalType.List.Select(x => new GoalTypeLookupDto(x.Value, x.Name)).ToList()));
  }
}
