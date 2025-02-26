using GoalManager.Core.GoalManagement;

namespace GoalManager.UseCases.GoalManagement.GetGoalValueTypeLookup;

internal sealed class GetGoalValueTypeLookupQueryHandler : IQueryHandler<GetGoalValueTypeLookupQuery, Result<List<GoalValueTypeLookupDto>>>
{
  public Task<Result<List<GoalValueTypeLookupDto>>> Handle(GetGoalValueTypeLookupQuery request, CancellationToken cancellationToken)
  {
    return Task.FromResult(
      new Result<List<GoalValueTypeLookupDto>>(
        GoalValueType.List.Select(x => new GoalValueTypeLookupDto(x.Value, x.Name)).ToList()));
  }
}
