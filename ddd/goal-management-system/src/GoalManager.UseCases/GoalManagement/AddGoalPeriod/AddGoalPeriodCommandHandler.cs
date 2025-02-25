using GoalManager.Core;
using GoalManager.Core.GoalManagement;
using GoalManager.Core.GoalManagement.Specifications;

namespace GoalManager.UseCases.GoalManagement.AddGoalPeriod;

public sealed class AddGoalPeriodCommandHandler(IRepository<GoalPeriod> goalPeriodRepository) : ICommandHandler<AddGoalPeriodCommand, Result<int>>
{
  public async Task<Result<int>> Handle(AddGoalPeriodCommand request, CancellationToken cancellationToken)
  {
    if (await goalPeriodRepository.AnyAsync(new GoalPeriodByTeamIdAndYearSpec(request.TeamId, request.Year), cancellationToken).ConfigureAwait(false))
    {
      return Result.Error($"{request.Year} Goal Period for the specified team already exists");
    }

    var createGoalPeriodResult = GoalPeriod.Create(request.TeamId, request.Year);
    if (!createGoalPeriodResult.IsSuccess)
    {
      return createGoalPeriodResult.ToResult();
    }

    var goalPeriod = await goalPeriodRepository.AddAsync(createGoalPeriodResult.Value, cancellationToken).ConfigureAwait(false);
    return Result<int>.Success(goalPeriod.Id, $"{request.Year} Goal Period is created");
  }
}
