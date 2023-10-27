using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wheel.Migrations
{
    /// <inheritdoc />
    public partial class Add_FileStorage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreationTime",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2023, 10, 20, 9, 43, 31, 836, DateTimeKind.Unspecified).AddTicks(2947), new TimeSpan(0, 8, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "TEXT",
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 10, 18, 11, 10, 55, 862, DateTimeKind.Unspecified).AddTicks(5120), new TimeSpan(0, 8, 0, 0, 0)));

            migrationBuilder.CreateTable(
                name: "FileStorages",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FileName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    ContentType = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                    FileStorageType = table.Column<int>(type: "INTEGER", nullable: false),
                    Size = table.Column<long>(type: "INTEGER", nullable: false),
                    Path = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    CreationTime = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    Provider = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileStorages", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FileStorages");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreationTime",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2023, 10, 18, 11, 10, 55, 862, DateTimeKind.Unspecified).AddTicks(5120), new TimeSpan(0, 8, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "TEXT",
                oldDefaultValue: new DateTimeOffset(new DateTime(2023, 10, 20, 9, 43, 31, 836, DateTimeKind.Unspecified).AddTicks(2947), new TimeSpan(0, 8, 0, 0, 0)));
        }
    }
}
