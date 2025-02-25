using GoalManager.Core.GoalManagement;

namespace GoalManager.Infrastructure.Data.Config.GoalManagement;

internal sealed class GoalPeriodConfiguration : IEntityTypeConfiguration<GoalPeriod>
{
  public void Configure(EntityTypeBuilder<GoalPeriod> builder)
  {
    builder.HasKey(p => p.Id);

    builder.Property(p => p.TeamId);
    builder.Property(p => p.Year);

    builder.HasIndex(p => new { p.TeamId, p.Year }).IsUnique();
  }
}
