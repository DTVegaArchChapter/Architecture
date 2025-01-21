namespace GoalManager.UseCases.Organisation.ListOrganisations;

public record ListOrganisationsQuery(int? Skip, int? Take) : IQuery<Result<List<OrganisationListItemDto>>>;
