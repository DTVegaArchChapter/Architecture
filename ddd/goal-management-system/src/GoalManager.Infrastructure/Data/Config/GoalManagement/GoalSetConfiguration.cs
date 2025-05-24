using GoalManager.Core.GoalManagement;

namespace GoalManager.Infrastructure.Data.Config.GoalManagement;

internal sealed class GoalSetConfiguration : IEntityTypeConfiguration<GoalSet>
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

    builder.Property(gs => gs.CharacterPoint)
      .IsRequired(false)
      .HasMaxLength(5);

    builder.HasMany(gs => gs.Goals)
      .WithOne()
      .HasForeignKey(g => g.GoalSetId)
      .OnDelete(DeleteBehavior.Cascade);

    builder.HasIndex(gs => new { gs.TeamId, gs.PeriodId, gs.UserId }).IsUnique();
  }
}
