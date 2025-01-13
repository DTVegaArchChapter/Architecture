namespace GoalManager.UseCases.Organisation.AddTeam;

public record AddTeamCommand(int organisationId, string teamName) : ICommand<Result>;
