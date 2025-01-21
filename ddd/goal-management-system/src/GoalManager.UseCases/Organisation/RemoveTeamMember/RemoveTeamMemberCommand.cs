namespace GoalManager.UseCases.Organisation.RemoveTeamMember;

public record RemoveTeamMemberCommand(int OrganisationId, int TeamId, int UserId) : ICommand<Result>;
