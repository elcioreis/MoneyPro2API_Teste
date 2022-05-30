using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoneyPro2.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Coin",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nickname = table.Column<string>(type: "VARCHAR(25)", maxLength: 25, nullable: false),
                    Symbol = table.Column<string>(type: "VARCHAR(10)", maxLength: 10, nullable: false),
                    Default = table.Column<bool>(type: "BIT", nullable: false),
                    Virtual = table.Column<bool>(type: "BIT", nullable: false),
                    Active = table.Column<bool>(type: "BIT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coin", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "VARCHAR(10)", maxLength: 10, nullable: false),
                    Slug = table.Column<string>(type: "VARCHAR(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "NVARCHAR(80)", maxLength: 80, nullable: false),
                    Email = table.Column<string>(type: "VARCHAR(160)", maxLength: 160, nullable: false),
                    Slug = table.Column<string>(type: "VARCHAR(160)", maxLength: 160, nullable: false),
                    ControlStart = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETDATE()"),
                    PasswordHash = table.Column<string>(type: "VARCHAR(64)", maxLength: 64, nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Login",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    LoginDate = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETDATE()"),
                    IpAddress = table.Column<string>(type: "VARCHAR(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Login", x => x.Id);
                    table.ForeignKey(
                        name: "Fk_Login_User",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRole",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "INT", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRole", x => new { x.RoleId, x.UserId });
                    table.ForeignKey(
                        name: "FK_UserRole_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRole_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Coin",
                columns: new[] { "Id", "Active", "Default", "Nickname", "Symbol", "Virtual" },
                values: new object[] { 1, true, true, "Real Brasileiro", "R$", false });

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "Name", "Slug" },
                values: new object[] { 1, "admin", "admin" });

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "Name", "Slug" },
                values: new object[] { 2, "user", "user" });

            migrationBuilder.CreateIndex(
                name: "IX_Coin_Nickname",
                table: "Coin",
                column: "Nickname",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Coin_Symbol",
                table: "Coin",
                column: "Symbol",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Login_UserId_LoginDate",
                table: "Login",
                columns: new[] { "UserId", "LoginDate" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Role_Name",
                table: "Role",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Role_Slug",
                table: "Role",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_Email",
                table: "User",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_Slug",
                table: "User",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_UserId",
                table: "UserRole",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Coin");

            migrationBuilder.DropTable(
                name: "Login");

            migrationBuilder.DropTable(
                name: "UserRole");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
