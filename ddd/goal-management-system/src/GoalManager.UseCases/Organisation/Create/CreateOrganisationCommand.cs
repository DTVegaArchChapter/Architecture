namespace GoalManager.UseCases.Organisation.Create;

public record CreateOrganisationCommand(string Name) : ICommand<Result<int>>;
