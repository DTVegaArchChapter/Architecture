using GoalManager.Core;
using GoalManager.Core.GoalManagement;
using GoalManager.Core.GoalManagement.Specifications;
using GoalManager.Core.Exceptions;
using Polly;
using Polly.Retry;

namespace GoalManager.UseCases.GoalManagement.UpdateGoal;

internal sealed class UpdateGoalCommandHandler(IRepository<GoalSet> goalSetRepository) : ICommandHandler<UpdateGoalCommand, Result<(int GoalSetId, int GoalId)>>
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

  public async Task<Result<(int GoalSetId, int GoalId)>> Handle(UpdateGoalCommand request, CancellationToken cancellationToken)
  {
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

        var updateGoalResult = goalSet.UpdateGoal(request.GoalId, request.Title, request.GoalType, request.GoalValueType, request.Percentage);
        if (!updateGoalResult.IsSuccess)
        {
          // Business rule failure; do not retry.
          return updateGoalResult.ToResult();
        }

        await goalSetRepository.UpdateAsync(goalSet, token).ConfigureAwait(false);
        return Result.Success((goalSet.Id, request.GoalId));
      }, cancellationToken).ConfigureAwait(false);
    }
    catch (ConcurrencyException)
    {
      return Result.Error("Another user modified this goal set at the same time. Your change could not be applied automatically. Please try again.");
    }
  }
}
