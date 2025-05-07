using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoalManager.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddProgressIdToGoal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProgressId",
                table: "Goal",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Goal_ProgressId",
                table: "Goal",
                column: "ProgressId",
                unique: true,
                filter: "[ProgressId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Goal_GoalProgress_ProgressId",
                table: "Goal",
                column: "ProgressId",
                principalTable: "GoalProgress",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Goal_GoalProgress_ProgressId",
                table: "Goal");

            migrationBuilder.DropIndex(
                name: "IX_Goal_ProgressId",
                table: "Goal");

            migrationBuilder.DropColumn(
                name: "ProgressId",
                table: "Goal");
        }
    }
}
