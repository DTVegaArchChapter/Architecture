using GoalManager.Core.GoalManagement.Events;
using GoalManager.Core.Interfaces;
using GoalManager.UseCases.Identity;
using GoalManager.UseCases.Organisation;

using MediatR;

namespace GoalManager.UseCases.GoalManagement.UpdateGoalProgress;

internal sealed class GoalProgressAddedEventHandler(
  IOrganisationQueryService organisationQueryService,
  IIdentityQueryService identityQueryService,
  IEmailSender emailSender) : INotificationHandler<GoalProgressAddedEvent>
{
  public async Task Handle(GoalProgressAddedEvent notification, CancellationToken cancellationToken)
  {
    var teamLeaderUserIds = await organisationQueryService.GetTeamLeaderUserIdsAsync(notification.TeamId).ConfigureAwait(false);
    var teamLeaderEmails = await identityQueryService.GetUserEmails(teamLeaderUserIds).ConfigureAwait(false);
    var userName = await identityQueryService.GetUserName(notification.UserId).ConfigureAwait(false) ?? "unknown";
   
    foreach (var email in teamLeaderEmails)
    {
      await emailSender.SendEmailAsync(
        email,
        "info@goalmanager.test",
        "Goal Progress is Waiting for Approval",
        $"{userName} user updated the '{notification.GoalName}' goal progress. Goal progress is waiting for approval.").ConfigureAwait(false);
    }
  }
}
