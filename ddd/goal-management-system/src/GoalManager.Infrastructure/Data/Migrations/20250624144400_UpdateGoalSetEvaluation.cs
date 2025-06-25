using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoalManager.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateGoalSetEvaluation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_GoalSetEvaluation_GoalSetId",
                table: "GoalSetEvaluation");

            migrationBuilder.CreateIndex(
                name: "IX_GoalSetEvaluation_GoalSetId",
                table: "GoalSetEvaluation",
                column: "GoalSetId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_GoalSetEvaluation_GoalSetId",
                table: "GoalSetEvaluation");

            migrationBuilder.CreateIndex(
                name: "IX_GoalSetEvaluation_GoalSetId",
                table: "GoalSetEvaluation",
                column: "GoalSetId");
        }
    }
}
