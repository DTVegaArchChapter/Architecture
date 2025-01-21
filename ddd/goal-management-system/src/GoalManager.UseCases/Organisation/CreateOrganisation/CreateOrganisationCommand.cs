namespace GoalManager.UseCases.Organisation.CreateOrganisation;

public record CreateOrganisationCommand(string Name) : ICommand<Result<int>>;
