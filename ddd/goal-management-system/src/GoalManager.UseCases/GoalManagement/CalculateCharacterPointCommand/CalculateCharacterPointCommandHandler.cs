using GoalManager.Core.GoalManagement;
using GoalManager.Core.GoalManagement.Specifications;

namespace GoalManager.UseCases.GoalManagement.CalculateCharacterPointCommand;

public class CalculateCharacterPointCommandHandler(IRepository<GoalSet> goalSetRepository) : ICommandHandler<CalculateCharacterPointCommand, Result>
{
  public async Task<Result> Handle(CalculateCharacterPointCommand request, CancellationToken cancellationToken)
  {
    var goalSets = await goalSetRepository.ListAsync(new GoalSetsWithGoalsByTeamIdSpec(request.TeamId));


    var isCanCalculate = !goalSets.Any(x => x.Goals.Any(g => g.Point == null));

    if(!isCanCalculate)
    {
      return Result.Error("Not all goals for the team have been calculated");

    }

    CalculateCharacterPoint(goalSets);

    await goalSetRepository.UpdateRangeAsync(goalSets);

    return Result.Success();
  }


  private void  CalculateCharacterPoint(List<GoalSet> goalSets)
  {
    var scores = goalSets.Select(x => x.Point!.Value).ToList(); 
    double avg = scores.Average();
    double stdDev = Math.Sqrt(scores.Sum(s => Math.Pow(s - avg, 2)) / scores.Count);

    foreach (var goal in goalSets)
    {
      var score = goal.Point!.Value;
      double z = (score - avg) / stdDev;
      string grade = z switch
      {
        > 1.5 => "A",
        > 0.5 => "B",
        > -0.5 => "C",
        > -1.5 => "D",
        _ => "F"
      };

      goal.SetCharacterPoint(grade); // Harf notunu set ediyoruz
    }
  }
}
