﻿using GoalManager.Core.GoalManagement;
using GoalManager.Infrastructure.Data;
using GoalManager.UseCases.GoalManagement;
using GoalManager.UseCases.GoalManagement.GetGoalSet;
using GoalManager.UseCases.GoalManagement.GetPendingApprovalGoals;
using GoalManager.UseCases.GoalManagement.GetPendingApprovalGoalSets;
using GoalManager.UseCases.GoalManagement.GetTeamGoalSetListsOfTeamLeader;
using GoalManager.UseCases.GoalManagement.GetTeamPerformanceData;

namespace GoalManager.Infrastructure.Queries.GoalManagement;

public sealed class GoalManagementQueryService(AppDbContext appDbContext) : IGoalManagementQueryService
{
  public async Task<List<PendingApprovalGoalDto>> GetPendingApprovalGoals(IList<int> teamIds)
  {
    var results = await (
        from goal in appDbContext.Goal.AsNoTracking()
        join goalSet in appDbContext.GoalSet.AsNoTracking() on goal.GoalSetId equals goalSet.Id
        where teamIds.Contains(goalSet.TeamId) && goal.GoalProgress!.Status == GoalProgressStatus.WaitingForApproval
        select new
        {
          GoalId = goal.Id,
          goal.Title,
          goal.GoalValue.MinValue,
          goal.GoalValue.MaxValue,
          goal.GoalProgress!.ActualValue,
          goal.GoalProgress!.Comment,
          goal.GoalProgress!.Status,
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
      UserId = x.UserId,
      TeamId = x.TeamId
    }).ToList();
  }

  public Task<List<PendingApprovalGoalSetDto>> GetPendingApprovalGoalSets(IList<int> teamIds)
  {
    return (from goalSet in appDbContext.GoalSet.AsNoTracking()
            where teamIds.Contains(goalSet.TeamId) && goalSet.Status == GoalSetStatus.WaitingForApproval
            select new PendingApprovalGoalSetDto
                   {
                     UserId = goalSet.UserId, 
                     TeamId = goalSet.TeamId, 
                     GoalSetId = goalSet.Id
                   }).ToListAsync();
  }

  public async Task<TeamPerformanceDataDto> GetTeamPerformanceData(int teamId)
  {
    var teamMemberPerformanceData = await appDbContext.GoalSet.AsNoTracking()
                                      .Where(goalSet => goalSet.TeamId == teamId)
                                      .Select(
                                        goalSet => new TeamMemberPerformanceDataDto
                                                   {
                                                     GoalSetId = goalSet.Id,
                                                     UserId = goalSet.UserId,
                                                     GoalSetStatus = goalSet.Status,
                                                     Year = goalSet.GoalPeriod.Year,
                                                     TeamId = goalSet.TeamId,
                                                     Goals = goalSet.Goals.Select(
                                                         x => new GoalPerformanceDataDto
                                                              {
                                                                Title = x.Title,
                                                                Percentage = x.Percentage,
                                                                ActualValue = x.ActualValue,
                                                                GoalType = x.GoalType,
                                                                GoalValue = x.GoalValue
                                                              })
                                                       .ToList()
                                                   })
      .ToListAsync();

    return new TeamPerformanceDataDto
           {
             TeamId = teamId, 
             TeamMembersPerformanceData = teamMemberPerformanceData
           };
  }

  public Task<List<TeamMemberGoalSetListItemDto>> GetTeamMemberGoalSetsList(IList<int> teamIds)
  {
    return (
            from goalSet in appDbContext.GoalSet.AsNoTracking() 
            join goalPeriod in appDbContext.GoalPeriod on goalSet.TeamId equals goalPeriod.TeamId 
            where teamIds.Contains(goalSet.TeamId) && goalPeriod.Year == DateTime.Now.Year
            select new TeamMemberGoalSetListItemDto
                  {
                     Status = goalSet.Status,
                     TeamId = goalSet.TeamId,
                     UserId = goalSet.UserId,
                     GoalSetId = goalSet.Id
                   }).ToListAsync();
  }

  public Task<GoalSetDto?> GetGoalSet(int teamId, int year, int userId)
  {
    return (from goalSet in appDbContext.GoalSet.AsNoTracking()
            join goalPeriod in appDbContext.GoalPeriod on goalSet.TeamId equals goalPeriod.TeamId
            where goalPeriod.TeamId == teamId && goalPeriod.Year == year && goalSet.UserId == userId
            select new GoalSetDto
                   {
                     Id = goalSet.Id,
                     TeamId = goalSet.TeamId,
                     UserId = goalSet.UserId,
                     Status = goalSet.Status,
                     Goals = goalSet.Goals.Select(g => new GoalDto
                                                       {
                                                         Id = g.Id,
                                                         Title = g.Title,
                                                         GoalType = g.GoalType,
                                                         GoalValue = g.GoalValue,
                                                         ActualValue = g.ActualValue,
                                                         Percentage = g.Percentage,
                                                         Status = g.GoalProgress == null ? null : g.GoalProgress.Status,
                                                         Comment = g.GoalProgress == null ? null : g.GoalProgress.Comment
                     }).ToList()
                   }).SingleOrDefaultAsync();
  }
}
