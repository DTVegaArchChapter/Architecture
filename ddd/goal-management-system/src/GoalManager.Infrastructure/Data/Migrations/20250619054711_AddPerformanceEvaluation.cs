using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoalManager.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPerformanceEvaluation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CharacterPoint",
                table: "GoalSet");

            migrationBuilder.DropColumn(
                name: "Point",
                table: "Goal");

            migrationBuilder.CreateTable(
                name: "GoalSetEvaluation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GoalSetId = table.Column<int>(type: "int", nullable: false),
                    PerformanceScore = table.Column<double>(type: "float", nullable: true),
                    PerformanceGrade = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoalSetEvaluation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GoalEvaluation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GoalTitle = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Point = table.Column<double>(type: "float", nullable: true),
                    Percentage = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    ActualValue = table.Column<int>(type: "int", nullable: false),
                    GoalSetEvaluationId = table.Column<int>(type: "int", nullable: false),
                    GoalValue_MaxValue = table.Column<int>(type: "int", nullable: false),
                    GoalValue_MidValue = table.Column<int>(type: "int", nullable: false),
                    GoalValue_MinValue = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoalEvaluation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GoalEvaluation_GoalSetEvaluation_GoalSetEvaluationId",
                        column: x => x.GoalSetEvaluationId,
                        principalTable: "GoalSetEvaluation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GoalEvaluation_GoalSetEvaluationId",
                table: "GoalEvaluation",
                column: "GoalSetEvaluationId");

            migrationBuilder.CreateIndex(
                name: "IX_GoalEvaluation_GoalTitle",
                table: "GoalEvaluation",
                column: "GoalTitle");

            migrationBuilder.CreateIndex(
                name: "IX_GoalSetEvaluation_GoalSetId",
                table: "GoalSetEvaluation",
                column: "GoalSetId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GoalEvaluation");

            migrationBuilder.DropTable(
                name: "GoalSetEvaluation");

            migrationBuilder.AddColumn<string>(
                name: "CharacterPoint",
                table: "GoalSet",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Point",
                table: "Goal",
                type: "float",
                nullable: true);
        }
    }
}
