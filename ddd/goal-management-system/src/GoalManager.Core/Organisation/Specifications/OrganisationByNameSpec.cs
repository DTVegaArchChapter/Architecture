namespace GoalManager.Core.Organisation.Specifications;

public class OrganisationByNameSpec : Specification<Organisation>
{
  public OrganisationByNameSpec(string name) => Query.Where(contributor => contributor.Name == name);
}
