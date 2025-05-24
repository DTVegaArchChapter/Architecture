using GoalManager.Core.GoalManagement;
using GoalManager.Infrastructure.Data;
using GoalManager.UseCases.GoalManagement;
using GoalManager.UseCases.GoalManagement.GetPendingApprovalGoals;
using GoalManager.UseCases.GoalManagement.GetPendingLastApprovalGoalSets;

namespace GoalManager.Infrastructure.Queries.GoalManagement;

public sealed class GoalManagementQueryService(AppDbContext appDbContext) : IGoalManagementQueryService
{
  public async Task<List<PendingApprovalGoalDto>> GetPendingApprovalGoals(IList<int> teamIds)
  {

    var results = await (
        from goal in appDbContext.Goal
        join goalSet in appDbContext.GoalSet on goal.GoalSetId equals goalSet.Id
        where teamIds.Contains(goalSet.TeamId) && (goal.GoalProgress!.Status == GoalProgressStatus.WaitingForApproval)
        select new
        {
          GoalId = goal.Id,
          goal.Title,
          goal.GoalValue.MinValue,
          goal.GoalValue.MaxValue,
          goal.GoalProgress!.ActualValue,
          goal.GoalProgress!.Comment,
          goal.GoalProgress!.Status,
          goal.Point,
          goalSet.UserId,
          goalSet.TeamId,
          GoalSetId = goalSet.Id
        })
        .ToListAsync();

    return results.Select(x => new PendingApprovalGoalDto
    {
      GoalId = x.GoalId,
      GoalSetId = x.GoalSetId,
      GoalTitle = x.Title,
      MinValue = x.MinValue,
      MaxValue = x.MaxValue,
      ActualValue = x.ActualValue,
      Comment = x.Comment,
      GoalProgressStatus = x.Status,
      Point = x.Point,
      UserId = x.UserId,
      TeamId = x.TeamId
    }).ToList();
  }

  public async Task<List<GetPendingLastApprovalGoalSetsDto>> GetPendingLastApprovalGoalSets(IList<int> teamIds)
  {
    var results = await(
            from goalSet in appDbContext.GoalSet
            where teamIds.Contains(goalSet.TeamId) && (goalSet.Status == GoalSetStatus.WaitingForLastApproval)
            select new GetPendingLastApprovalGoalSetsDto
            {
              GoalSetId = goalSet.Id,
              Status = goalSet.Status,
              TeamId = goalSet.TeamId,
              TeamName = string.Empty,
              UserId = goalSet.UserId,
              User = string.Empty
            })
            .ToListAsync();

    return results;
  }
}
