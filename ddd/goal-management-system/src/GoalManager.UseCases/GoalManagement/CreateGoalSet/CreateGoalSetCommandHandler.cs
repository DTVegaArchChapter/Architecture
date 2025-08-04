using GoalManager.Core.GoalManagement;
using GoalManager.Core.GoalManagement.Specifications;

namespace GoalManager.UseCases.GoalManagement.CreateGoalSet;

internal sealed class CreateGoalSetCommandHandler(
  IRepository<GoalPeriod> goalPeriodRepository, 
  IRepository<GoalSet> goalSetRepository) : ICommandHandler<CreateGoalSetCommand, Result>
{
  public async Task<Result> Handle(CreateGoalSetCommand request, CancellationToken cancellationToken)
  {
    var period = await goalPeriodRepository.SingleOrDefaultAsync(new GoalPeriodByTeamIdAndYearSpec(request.TeamId, request.Year), cancellationToken);
    if (period == null)
    {
      return Result.Error($"{request.Year} year's goal period is not created for the specified team. Go to Update Team page and click Create Goal Period button.");
    }

    var newGoalSet = GoalSet.Create(request.TeamId, period.Id, request.UserId);
    await goalSetRepository.AddAsync(newGoalSet, cancellationToken).ConfigureAwait(false);

    return Result.Success();
  }
}
