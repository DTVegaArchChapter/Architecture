namespace GoalManager.UseCases.Organisation.AddTeam;

public record AddTeamCommand(int OrganisationId, string TeamName) : ICommand<Result>;
