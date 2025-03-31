using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoalManager.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GoalPeriod",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TeamId = table.Column<int>(type: "int", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoalPeriod", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GoalSet",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    PeriodId = table.Column<int>(type: "int", nullable: false),
                    TeamId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoalSet", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NotificationItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationItem", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Organisation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organisation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Goal",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    GoalType = table.Column<int>(type: "int", nullable: false),
                    ActualValue = table.Column<int>(type: "int", nullable: true),
                    GoalSetId = table.Column<int>(type: "int", nullable: false),
                    Percentage = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    GoalValue_GoalValueType = table.Column<int>(type: "int", nullable: false),
                    GoalValue_MaxValue = table.Column<int>(type: "int", nullable: false),
                    GoalValue_MidValue = table.Column<int>(type: "int", nullable: false),
                    GoalValue_MinValue = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Goal", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Goal_GoalSet_GoalSetId",
                        column: x => x.GoalSetId,
                        principalTable: "GoalSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Team",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    OrganisationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Team", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Team_Organisation_OrganisationId",
                        column: x => x.OrganisationId,
                        principalTable: "Organisation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GoalProgress",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GoalId = table.Column<int>(type: "int", nullable: false),
                    ActualValue = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoalProgress", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GoalProgress_Goal_GoalId",
                        column: x => x.GoalId,
                        principalTable: "Goal",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TeamMember",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TeamId = table.Column<int>(type: "int", nullable: false),
                    MemberType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamMember", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeamMember_Team_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Team",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Goal_GoalSetId",
                table: "Goal",
                column: "GoalSetId");

            migrationBuilder.CreateIndex(
                name: "IX_Goal_Title",
                table: "Goal",
                column: "Title");

            migrationBuilder.CreateIndex(
                name: "IX_GoalPeriod_TeamId_Year",
                table: "GoalPeriod",
                columns: new[] { "TeamId", "Year" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GoalProgress_GoalId",
                table: "GoalProgress",
                column: "GoalId");

            migrationBuilder.CreateIndex(
                name: "IX_GoalSet_TeamId_PeriodId_UserId",
                table: "GoalSet",
                columns: new[] { "TeamId", "PeriodId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Organisation_Name",
                table: "Organisation",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Team_OrganisationId_Name",
                table: "Team",
                columns: new[] { "OrganisationId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TeamMember_TeamId",
                table: "TeamMember",
                column: "TeamId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GoalPeriod");

            migrationBuilder.DropTable(
                name: "GoalProgress");

            migrationBuilder.DropTable(
                name: "NotificationItem");

            migrationBuilder.DropTable(
                name: "TeamMember");

            migrationBuilder.DropTable(
                name: "Goal");

            migrationBuilder.DropTable(
                name: "Team");

            migrationBuilder.DropTable(
                name: "GoalSet");

            migrationBuilder.DropTable(
                name: "Organisation");
        }
    }
}
