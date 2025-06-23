namespace GoalManager.UseCases.GoalManagement.GetTeamGoalSetListsOfTeamLeader;

public record GetTeamGoalSetListsOfTeamLeaderQuery(int TeamLeaderUserId) : IQuery<Result<List<TeamGoalSetListItem>>>;
