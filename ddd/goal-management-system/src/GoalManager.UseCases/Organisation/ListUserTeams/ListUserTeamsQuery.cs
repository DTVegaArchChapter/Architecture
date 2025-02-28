namespace GoalManager.UseCases.Organisation.ListUserTeams;

public record ListUserTeamsQuery(int userId) : IQuery<List<UserTeamListItemDto>>;
