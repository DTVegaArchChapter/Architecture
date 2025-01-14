namespace GoalManager.UseCases.Organisation.UpdateTeam;

public record UpdateTeamCommand(int OrganisationId, int TeamId, string TeamName) : ICommand<Result>;
