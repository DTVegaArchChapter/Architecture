using GoalManager.Core.Notification;

namespace GoalManager.Infrastructure.Data.Config;

internal class NotificationConfiguration : IEntityTypeConfiguration<NotificationItem>
{
  public void Configure(EntityTypeBuilder<NotificationItem> builder)
  {
    builder.HasKey(x => x.Id);
    builder.Property(p => p.Text).IsRequired();
    builder.Property(p => p.CreateDate).IsRequired();
  }
}
