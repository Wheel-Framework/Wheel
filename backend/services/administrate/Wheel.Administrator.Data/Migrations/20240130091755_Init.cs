using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wheel.Administrator.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BackendRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BackendRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BackendUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    CreationTime = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    UserName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: true),
                    SecurityStamp = table.Column<string>(type: "TEXT", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BackendUsers", x => x.Id);
                });

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
                    Icon = table.Column<string>(type: "TEXT", maxLength: 128, nullable: true),
                    Sort = table.Column<int>(type: "INTEGER", nullable: false),
                    ParentId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Menus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Menus_Menus_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Menus",
                        principalColumn: "Id");
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
                name: "BackendRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RoleId = table.Column<string>(type: "TEXT", nullable: false),
                    ClaimType = table.Column<string>(type: "TEXT", nullable: true),
                    ClaimValue = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BackendRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BackendRoleClaims_BackendRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "BackendRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BackendUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    ClaimType = table.Column<string>(type: "TEXT", nullable: true),
                    ClaimValue = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BackendUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BackendUserClaims_BackendUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "BackendUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BackendUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "TEXT", nullable: true),
                    UserId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BackendUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_BackendUserLogins_BackendUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "BackendUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BackendUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    RoleId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BackendUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_BackendUserRoles_BackendRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "BackendRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BackendUserRoles_BackendUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "BackendUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BackendUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    LoginProvider = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    Value = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BackendUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_BackendUserTokens_BackendUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "BackendUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoleMenus",
                columns: table => new
                {
                    RoleId = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false),
                    MenuId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleMenus", x => new { x.MenuId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_RoleMenus_BackendRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "BackendRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoleMenus_Menus_MenuId",
                        column: x => x.MenuId,
                        principalTable: "Menus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                name: "IX_BackendRoleClaims_RoleId",
                table: "BackendRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "BackendRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BackendUserClaims_UserId",
                table: "BackendUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BackendUserLogins_UserId",
                table: "BackendUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BackendUserRoles_RoleId",
                table: "BackendUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "BackendUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "BackendUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Menus_ParentId",
                table: "Menus",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleMenus_RoleId",
                table: "RoleMenus",
                column: "RoleId");

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
                name: "BackendRoleClaims");

            migrationBuilder.DropTable(
                name: "BackendUserClaims");

            migrationBuilder.DropTable(
                name: "BackendUserLogins");

            migrationBuilder.DropTable(
                name: "BackendUserRoles");

            migrationBuilder.DropTable(
                name: "BackendUserTokens");

            migrationBuilder.DropTable(
                name: "FileStorages");

            migrationBuilder.DropTable(
                name: "PermissionGrants");

            migrationBuilder.DropTable(
                name: "RoleMenus");

            migrationBuilder.DropTable(
                name: "SettingValues");

            migrationBuilder.DropTable(
                name: "BackendUsers");

            migrationBuilder.DropTable(
                name: "BackendRoles");

            migrationBuilder.DropTable(
                name: "Menus");

            migrationBuilder.DropTable(
                name: "SettingGroups");
        }
    }
}
