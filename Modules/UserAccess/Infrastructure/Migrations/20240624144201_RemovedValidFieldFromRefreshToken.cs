using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modules.UserAccess.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemovedValidFieldFromRefreshToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Valid",
                schema: "UserAccess",
                table: "RefreshToken");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Valid",
                schema: "UserAccess",
                table: "RefreshToken",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
