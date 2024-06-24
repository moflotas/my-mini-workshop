using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modules.UserAccess.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "UserAccess");

            migrationBuilder.CreateTable(
                name: "InboxMessage",
                schema: "UserAccess",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OccurredOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    Data = table.Column<string>(type: "text", nullable: false),
                    ProcessedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InboxMessage", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InternalCommand",
                schema: "UserAccess",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OccurredOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    Data = table.Column<string>(type: "text", nullable: false),
                    ProcessedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InternalCommand", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OutboxMessage",
                schema: "UserAccess",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OccurredOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    Data = table.Column<string>(type: "text", nullable: false),
                    ProcessedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutboxMessage", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Permission",
                schema: "UserAccess",
                columns: table => new
                {
                    Code = table.Column<string>(type: "text", nullable: false),
                    DisplayName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permission", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "RefreshToken",
                schema: "UserAccess",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Valid = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshToken", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                schema: "UserAccess",
                columns: table => new
                {
                    Code = table.Column<string>(type: "text", nullable: false),
                    DisplayName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "User",
                schema: "UserAccess",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    FirstNameRu = table.Column<string>(type: "text", nullable: true),
                    LastNameRu = table.Column<string>(type: "text", nullable: true),
                    PatronymicRu = table.Column<string>(type: "text", nullable: true),
                    FirstNameEn = table.Column<string>(type: "text", nullable: true),
                    LastNameEn = table.Column<string>(type: "text", nullable: true),
                    PatronymicEn = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PermissionRole",
                schema: "UserAccess",
                columns: table => new
                {
                    PermissionsCode = table.Column<string>(type: "text", nullable: false),
                    RoleCode = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionRole", x => new { x.PermissionsCode, x.RoleCode });
                    table.ForeignKey(
                        name: "FK_PermissionRole_Permission_PermissionsCode",
                        column: x => x.PermissionsCode,
                        principalSchema: "UserAccess",
                        principalTable: "Permission",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PermissionRole_Role_RoleCode",
                        column: x => x.RoleCode,
                        principalSchema: "UserAccess",
                        principalTable: "Role",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoleUser",
                schema: "UserAccess",
                columns: table => new
                {
                    RolesCode = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleUser", x => new { x.RolesCode, x.UserId });
                    table.ForeignKey(
                        name: "FK_RoleUser_Role_RolesCode",
                        column: x => x.RolesCode,
                        principalSchema: "UserAccess",
                        principalTable: "Role",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoleUser_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "UserAccess",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PermissionRole_RoleCode",
                schema: "UserAccess",
                table: "PermissionRole",
                column: "RoleCode");

            migrationBuilder.CreateIndex(
                name: "IX_RoleUser_UserId",
                schema: "UserAccess",
                table: "RoleUser",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InboxMessage",
                schema: "UserAccess");

            migrationBuilder.DropTable(
                name: "InternalCommand",
                schema: "UserAccess");

            migrationBuilder.DropTable(
                name: "OutboxMessage",
                schema: "UserAccess");

            migrationBuilder.DropTable(
                name: "PermissionRole",
                schema: "UserAccess");

            migrationBuilder.DropTable(
                name: "RefreshToken",
                schema: "UserAccess");

            migrationBuilder.DropTable(
                name: "RoleUser",
                schema: "UserAccess");

            migrationBuilder.DropTable(
                name: "Permission",
                schema: "UserAccess");

            migrationBuilder.DropTable(
                name: "Role",
                schema: "UserAccess");

            migrationBuilder.DropTable(
                name: "User",
                schema: "UserAccess");
        }
    }
}
