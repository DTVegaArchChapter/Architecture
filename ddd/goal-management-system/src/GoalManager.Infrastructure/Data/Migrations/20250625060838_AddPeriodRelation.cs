using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoalManager.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPeriodRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Year",
                table: "GoalSetEvaluation",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_GoalSet_PeriodId",
                table: "GoalSet",
                column: "PeriodId");

            migrationBuilder.AddForeignKey(
                name: "FK_GoalSet_GoalPeriod_PeriodId",
                table: "GoalSet",
                column: "PeriodId",
                principalTable: "GoalPeriod",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GoalSet_GoalPeriod_PeriodId",
                table: "GoalSet");

            migrationBuilder.DropIndex(
                name: "IX_GoalSet_PeriodId",
                table: "GoalSet");

            migrationBuilder.DropColumn(
                name: "Year",
                table: "GoalSetEvaluation");
        }
    }
}
