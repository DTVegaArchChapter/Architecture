namespace GoalManager.UseCases.Organisation.GetTeamForUpdate;

public record GetTeamForUpdateQuery(int Id) : IQuery<Result<TeamForUpdateDto>>;
