using GoalManager.Core.Interfaces;

namespace GoalManager.Core.GoalManagement.Events;

public sealed class GoalSetSentToApprovalEvent(int id, int teamId, int userId) : DomainEventBase, IHasNotificationText
{
  public int Id { get; } = id;

  public int TeamId { get; } = teamId;

  public int UserId { get; } = userId;

  public string GetNotificationText()
  {
    return "GoalSet #{Id} is sent to approval for team #{TeamId} by user #{UserId}";
  }
}
