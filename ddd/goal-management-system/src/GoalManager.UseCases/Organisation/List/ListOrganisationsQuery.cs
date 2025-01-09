namespace GoalManager.UseCases.Organisation.List;

public record ListOrganisationsQuery(int? Skip, int? Take) : IQuery<Result<List<OrganisationListItemDto>>>;
