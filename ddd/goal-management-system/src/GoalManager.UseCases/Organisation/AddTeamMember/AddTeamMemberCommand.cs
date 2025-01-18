namespace GoalManager.UseCases.Organisation.AddTeamMember;

public record AddTeamMemberCommand(int OrganisationId, int TeamId, int UserId) : ICommand<Result>;
