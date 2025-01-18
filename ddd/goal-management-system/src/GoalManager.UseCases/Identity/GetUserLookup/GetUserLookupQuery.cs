namespace GoalManager.UseCases.Identity.GetUserLookup;

public record GetUserLookupQuery : IQuery<Result<List<UserLookupDto>>>;
