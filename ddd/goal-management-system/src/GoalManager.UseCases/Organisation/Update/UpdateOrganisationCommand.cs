namespace GoalManager.UseCases.Organisation.Update;

public record UpdateOrganisationCommand(int Id, string Name) : ICommand<Result>;
