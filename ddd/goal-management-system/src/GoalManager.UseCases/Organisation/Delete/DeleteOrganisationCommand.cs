namespace GoalManager.UseCases.Organisation.Delete;

public record DeleteOrganisationCommand(int Id) : ICommand<Result>;
