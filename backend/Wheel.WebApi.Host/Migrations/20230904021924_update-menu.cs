using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wheel.Migrations
{
    /// <inheritdoc />
    public partial class updatemenu : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "MenuId",
                table: "Menus",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ParentId",
                table: "Menus",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Menus_MenuId",
                table: "Menus",
                column: "MenuId");

            migrationBuilder.AddForeignKey(
                name: "FK_Menus_Menus_MenuId",
                table: "Menus",
                column: "MenuId",
                principalTable: "Menus",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Menus_Menus_MenuId",
                table: "Menus");

            migrationBuilder.DropIndex(
                name: "IX_Menus_MenuId",
                table: "Menus");

            migrationBuilder.DropColumn(
                name: "MenuId",
                table: "Menus");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "Menus");
        }
    }
}
