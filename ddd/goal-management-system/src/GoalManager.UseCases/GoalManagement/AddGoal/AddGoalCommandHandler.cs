using GoalManager.Core;
using GoalManager.Core.GoalManagement;
using GoalManager.Core.GoalManagement.Specifications;
using GoalManager.Core.Exceptions;
using Polly;
using Polly.Retry;

namespace GoalManager.UseCases.GoalManagement.AddGoal;

internal sealed class AddGoalCommandHandler(IRepository<GoalSet> goalSetRepository) : ICommandHandler<AddGoalCommand, Result<(int TeamId, int PeriodId)>>
{
  private static readonly ResiliencePipeline RetryPipeline = new ResiliencePipelineBuilder()
    .AddRetry(new RetryStrategyOptions
    {
      ShouldHandle = new PredicateBuilder().Handle<ConcurrencyException>(),
      MaxRetryAttempts = 3,
      Delay = TimeSpan.FromMilliseconds(50),
      BackoffType = DelayBackoffType.Exponential,
      UseJitter = true
    })
    .Build();

  public async Task<Result<(int TeamId, int PeriodId)>> Handle(AddGoalCommand request, CancellationToken cancellationToken)
  {
    // Validate once; independent of aggregate state.
    var goalValueResult = GoalValue.Create(request.MinValue, request.MidValue, request.MaxValue, request.GoalValueType);
    if (!goalValueResult.IsSuccess)
    {
      return goalValueResult.ToResult();
    }

    var spec = new GoalSetWithGoalsByGoalSetIdSpec(request.GoalSetId);

    try
    {
      return await RetryPipeline.ExecuteAsync(async token =>
      {
        var goalSet = await goalSetRepository.SingleOrDefaultAsync(spec, token).ConfigureAwait(false);
        if (goalSet == null)
        {
          return Result.Error($"Goal set not found for id: {request.GoalSetId}");
        }

        var addGoalResult = goalSet.AddGoal(request.Title, request.GoalType, goalValueResult.Value, request.Percentage);
        if (!addGoalResult.IsSuccess)
        {
          // Business rule failure; do not retry.
          return addGoalResult.ToResult();
        }

        await goalSetRepository.UpdateAsync(goalSet, token).ConfigureAwait(false);
        return Result.Success((goalSet.TeamId, goalSet.PeriodId));
      }, cancellationToken).ConfigureAwait(false);
    }
    catch (ConcurrencyException)
    {
      return Result.Error("Another user modified this goal set at the same time. Your change could not be applied automatically. Please try again.");
    }
  }
}
