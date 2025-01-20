using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoalManager.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddOrganisationIdTeamNameUniqueIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Team_OrganisationId",
                table: "Team");

            migrationBuilder.CreateIndex(
                name: "IX_Team_OrganisationId_Name",
                table: "Team",
                columns: new[] { "OrganisationId", "Name" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Team_OrganisationId_Name",
                table: "Team");

            migrationBuilder.CreateIndex(
                name: "IX_Team_OrganisationId",
                table: "Team",
                column: "OrganisationId");
        }
    }
}
