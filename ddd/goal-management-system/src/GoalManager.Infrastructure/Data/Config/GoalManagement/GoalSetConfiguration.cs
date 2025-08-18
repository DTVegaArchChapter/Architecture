using GoalManager.Core.GoalManagement;

namespace GoalManager.Infrastructure.Data.Config.GoalManagement;

internal sealed class GoalSetEvaluationConfiguration : IEntityTypeConfiguration<GoalSet>
{
  public void Configure(EntityTypeBuilder<GoalSet> builder)
  {
    builder.HasKey(gs => gs.Id);

    builder.Property(gs => gs.UserId)
      .IsRequired();

    builder.Property(gs => gs.PeriodId)
      .IsRequired();

    builder.Property(gs => gs.TeamId)
      .IsRequired();

    builder.HasMany(gs => gs.Goals)
      .WithOne()
      .HasForeignKey(g => g.GoalSetId)
      .OnDelete(DeleteBehavior.Cascade);

    builder.HasOne(gs => gs.GoalPeriod)
      .WithMany()
      .HasForeignKey(g => g.PeriodId);

    builder.HasIndex(gs => new { gs.TeamId, gs.PeriodId, gs.UserId }).IsUnique();

    // Configure optimistic concurrency token
    builder.Property(gs => gs.RowVersion)
      .IsRowVersion()
      .ValueGeneratedOnAddOrUpdate();
  }
}
