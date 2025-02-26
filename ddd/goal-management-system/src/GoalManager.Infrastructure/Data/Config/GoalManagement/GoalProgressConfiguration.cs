using GoalManager.Core.GoalManagement;

namespace GoalManager.Infrastructure.Data.Config.GoalManagement;

internal sealed class GoalProgressConfiguration : IEntityTypeConfiguration<GoalProgress>
{
  public void Configure(EntityTypeBuilder<GoalProgress> builder)
  {
    builder.HasKey(gp => gp.Id);

    builder.Property(gp => gp.GoalId)
      .IsRequired();

    builder.Property(gp => gp.ActualValue)
      .IsRequired();

    builder.Property(gp => gp.Comment)
      .HasMaxLength(500);

    builder.Property(gp => gp.Status)
      .HasConversion(p => p.Value, p => GoalProgressStatus.FromValue(p))
      .IsRequired();

    builder.HasOne(gp => gp.Goal)
      .WithMany(g => g.GoalProgressHistory)
      .HasForeignKey(gp => gp.GoalId)
      .OnDelete(DeleteBehavior.Cascade);
  }
}
