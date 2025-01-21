namespace GoalManager.UseCases.Organisation.UpdateOrganisations;

public record UpdateOrganisationCommand(int Id, string Name) : ICommand<Result>;
