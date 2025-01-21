namespace GoalManager.UseCases.Organisation.DeleteOrganisation;

public record DeleteOrganisationCommand(int Id) : ICommand<Result>;
