using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoalManager.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTeamMemberType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MemberType",
                table: "TeamMember",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MemberType",
                table: "TeamMember");
        }
    }
}
