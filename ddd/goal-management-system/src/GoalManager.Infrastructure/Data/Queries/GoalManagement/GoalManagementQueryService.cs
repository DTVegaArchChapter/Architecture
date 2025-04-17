using GoalManager.Core.GoalManagement;
using GoalManager.Core.Organisation;
using GoalManager.UseCases.GoalManagement;
using GoalManager.UseCases.GoalManagement.GetPendingApprovalGoals;
using GoalManager.UseCases.Organisation;
using Microsoft.EntityFrameworkCore;

namespace GoalManager.Infrastructure.Data.Queries.GoalManagement;

public sealed class GoalManagementQueryService(AppDbContext appDbContext, IOrganisationQueryService organisationQueryService) : IGoalManagementQueryService
{
  public async Task<List<PendingApprovalGoalDto>> GetPendingApprovalGoalsForTeamLeader(int teamLeaderUserId)
  {
    var teamMembers = await organisationQueryService.GetTeamMemberUserIdsByTeamLeader(teamLeaderUserId);
    if (!teamMembers.Any()) return [];

    var userIds = teamMembers.Keys.ToList();
    var teamIds = teamMembers.SelectMany(x => x.Value).Distinct().ToList();

    var results = await (
        from goal in appDbContext.Goal
        join goalSet in appDbContext.GoalSet on goal.GoalSetId equals goalSet.Id
        where userIds.Contains(goalSet.UserId)
           && teamIds.Contains(goalSet.TeamId)
        let latestProgress = goal.GoalProgressHistory
            .Where(p => p.Status == GoalProgressStatus.WaitingForApproval)
            .OrderByDescending(p => p.Id)
            .FirstOrDefault()
        where latestProgress != null
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

    var teamNamesDict = await organisationQueryService.GetTeamNamesAsync(teamIds);

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
