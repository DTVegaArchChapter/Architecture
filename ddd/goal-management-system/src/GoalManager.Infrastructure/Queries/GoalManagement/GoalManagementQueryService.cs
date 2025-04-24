using GoalManager.Core.GoalManagement;
using GoalManager.Core.Organisation;
using GoalManager.Infrastructure.Data;
using GoalManager.UseCases.GoalManagement;
using GoalManager.UseCases.GoalManagement.GetPendingApprovalGoals;
using GoalManager.UseCases.Organisation;
using Microsoft.EntityFrameworkCore;

namespace GoalManager.Infrastructure.Queries.GoalManagement;

public sealed class GoalManagementQueryService(AppDbContext appDbContext) : IGoalManagementQueryService
{
  public async Task<List<PendingApprovalGoalDto>> GetPendingApprovalGoalsForTeamLeader(int teamLeaderUserId, Dictionary<int, List<int>> teamMembers, Dictionary<int, string> teamNamesDict)
  {
    if (!teamMembers.Any()) return [];

    var userIds = teamMembers.Keys.ToList();
    var teamIds = teamMembers.SelectMany(x => x.Value).Distinct().ToList();

    var results = await (
        from goal in appDbContext.Goal
        join goalSet in appDbContext.GoalSet on goal.GoalSetId equals goalSet.Id
        where userIds.Contains(goalSet.UserId)
           && teamIds.Contains(goalSet.TeamId)
        let latestProgress = goal.GoalProgressHistory
            .OrderByDescending(p => p.Id)
            .FirstOrDefault()
        where latestProgress != null
           && latestProgress.Status == GoalProgressStatus.WaitingForApproval
           && goal.GoalProgressHistory
               .Where(p => p.Status == GoalProgressStatus.Approved || p.Status == GoalProgressStatus.Rejected)
               .All(p => p.Id < latestProgress.Id)
        select new
        {
          GoalId = goal.Id,
          goal.Title,
          goal.GoalValue.MinValue,
          goal.GoalValue.MaxValue,
          latestProgress.ActualValue,
          latestProgress.Comment,
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
      GoalOwnerUserId = x.UserId,
      TeamId = x.TeamId,
      TeamName = teamNamesDict.TryGetValue(x.TeamId, out var name) ? name : "Unknown Team"
    }).ToList();
  }
}
