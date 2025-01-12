namespace GoalManager.UseCases.Organisation.GetForUpdate;

public record GetOrganisationForUpdateQuery(int Id) : IQuery<Result<OrganisationForUpdateDto>>;
