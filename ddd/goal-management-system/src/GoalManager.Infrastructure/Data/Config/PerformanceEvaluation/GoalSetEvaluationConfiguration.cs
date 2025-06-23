using GoalManager.Core.PerformanceEvaluation;

namespace GoalManager.Infrastructure.Data.Config.PerformanceEvaluation;

internal sealed class GoalSetEvaluationConfiguration : IEntityTypeConfiguration<GoalSetEvaluation>
{
  public void Configure(EntityTypeBuilder<GoalSetEvaluation> builder)
  {
    builder.HasKey(gs => gs.Id);

    builder.Property(gs => gs.GoalSetId)
      .IsRequired();

    builder.Property(gs => gs.PerformanceGrade);
    builder.Property(gs => gs.PerformanceScore);

    builder.HasMany(gs => gs.GoalEvaluations)
      .WithOne()
      .HasForeignKey(g => g.GoalSetEvaluationId)
      .OnDelete(DeleteBehavior.Cascade);

    builder.HasIndex(gs => gs.GoalSetId);
  }
}
