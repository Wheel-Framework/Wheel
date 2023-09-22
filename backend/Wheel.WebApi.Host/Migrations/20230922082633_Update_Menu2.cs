using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wheel.Migrations
{
    /// <inheritdoc />
    public partial class Update_Menu2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "Menus");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ParentId",
                table: "Menus",
                type: "TEXT",
                nullable: true);
        }
    }
}
