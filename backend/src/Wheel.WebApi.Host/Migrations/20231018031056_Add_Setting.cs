using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wheel.Migrations
{
    /// <inheritdoc />
    public partial class Add_Setting : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreationTime",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2023, 10, 18, 11, 10, 55, 862, DateTimeKind.Unspecified).AddTicks(5120), new TimeSpan(0, 8, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "TEXT",
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 9, 28, 9, 54, 6, 21, DateTimeKind.Unspecified).AddTicks(7894), new TimeSpan(0, 8, 0, 0, 0)));

            migrationBuilder.CreateTable(
                name: "SettingGroups",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    NormalizedName = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SettingGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SettingValues",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SettingGroupId = table.Column<long>(type: "INTEGER", nullable: false),
                    Key = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    Value = table.Column<string>(type: "TEXT", nullable: false),
                    ValueType = table.Column<int>(type: "INTEGER", maxLength: 2048, nullable: false),
                    SettingScope = table.Column<int>(type: "INTEGER", nullable: false),
                    SettingScopeKey = table.Column<string>(type: "TEXT", maxLength: 128, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SettingValues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SettingValues_SettingGroups_SettingGroupId",
                        column: x => x.SettingGroupId,
                        principalTable: "SettingGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SettingGroups_Name",
                table: "SettingGroups",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_SettingValues_Key",
                table: "SettingValues",
                column: "Key");

            migrationBuilder.CreateIndex(
                name: "IX_SettingValues_SettingGroupId",
                table: "SettingValues",
                column: "SettingGroupId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SettingValues");

            migrationBuilder.DropTable(
                name: "SettingGroups");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreationTime",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2023, 9, 28, 9, 54, 6, 21, DateTimeKind.Unspecified).AddTicks(7894), new TimeSpan(0, 8, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "TEXT",
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 10, 18, 11, 10, 55, 862, DateTimeKind.Unspecified).AddTicks(5120), new TimeSpan(0, 8, 0, 0, 0)));
        }
    }
}
