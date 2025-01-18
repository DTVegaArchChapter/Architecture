namespace GoalManager.UseCases.Organisation.DeleteTeam;

public record DeleteTeamCommand(int OrganisationId, int TeamId) : ICommand<Result>;
