using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoalManager.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddYearUserIdTeamId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TeamId",
                table: "GoalSetEvaluation",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "GoalSetEvaluation",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TeamId",
                table: "GoalSetEvaluation");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "GoalSetEvaluation");
        }
    }
}
