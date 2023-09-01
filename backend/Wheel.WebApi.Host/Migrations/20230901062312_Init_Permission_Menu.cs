using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wheel.Migrations
{
    /// <inheritdoc />
    public partial class Init_Permission_Menu : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Resources_Cultures_CultureId",
                table: "Resources");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Resources",
                table: "Resources");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Cultures",
                table: "Cultures");

            migrationBuilder.RenameTable(
                name: "Resources",
                newName: "LocalizationResource");

            migrationBuilder.RenameTable(
                name: "Cultures",
                newName: "LocalizationCulture");

            migrationBuilder.RenameIndex(
                name: "IX_Resources_CultureId",
                table: "LocalizationResource",
                newName: "IX_LocalizationResource_CultureId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LocalizationResource",
                table: "LocalizationResource",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LocalizationCulture",
                table: "LocalizationCulture",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Menus",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    DisplayName = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    MenuType = table.Column<int>(type: "INTEGER", nullable: false),
                    Path = table.Column<string>(type: "TEXT", maxLength: 128, nullable: true),
                    Permission = table.Column<string>(type: "TEXT", maxLength: 128, nullable: true),
                    Sort = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Menus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PermissionGrants",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Permission = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    GrantType = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                    GrantValue = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionGrants", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_LocalizationResource_LocalizationCulture_CultureId",
                table: "LocalizationResource",
                column: "CultureId",
                principalTable: "LocalizationCulture",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LocalizationResource_LocalizationCulture_CultureId",
                table: "LocalizationResource");

            migrationBuilder.DropTable(
                name: "Menus");

            migrationBuilder.DropTable(
                name: "PermissionGrants");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LocalizationResource",
                table: "LocalizationResource");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LocalizationCulture",
                table: "LocalizationCulture");

            migrationBuilder.RenameTable(
                name: "LocalizationResource",
                newName: "Resources");

            migrationBuilder.RenameTable(
                name: "LocalizationCulture",
                newName: "Cultures");

            migrationBuilder.RenameIndex(
                name: "IX_LocalizationResource_CultureId",
                table: "Resources",
                newName: "IX_Resources_CultureId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Resources",
                table: "Resources",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Cultures",
                table: "Cultures",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Resources_Cultures_CultureId",
                table: "Resources",
                column: "CultureId",
                principalTable: "Cultures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
