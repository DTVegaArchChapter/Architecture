namespace GoalManager.UseCases.Organisation.GetTeamName;

public sealed record GetTeamNameQuery(int teamId) : IQuery<Result<string>>;
