using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoneyPro2.Migrations
{
    public partial class Institution : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Institution",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "INT", nullable: false),
                    InstitutionTypeId = table.Column<int>(type: "INT", nullable: false),
                    Nickname = table.Column<string>(type: "VARCHAR(25)", maxLength: 25, nullable: false),
                    Description = table.Column<string>(type: "VARCHAR(100)", maxLength: 100, nullable: false),
                    BankNumber = table.Column<int>(type: "INT", nullable: true),
                    Active = table.Column<bool>(type: "BIT", nullable: false, defaultValueSql: "1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Institution", x => x.Id);
                    table.ForeignKey(
                        name: "Fk_Institution_InstitutionType",
                        column: x => x.InstitutionTypeId,
                        principalTable: "InstitutionType",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "Fk_Institution_User",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Institution_InstitutionTypeId",
                table: "Institution",
                column: "InstitutionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Institution_UserId_Nickname",
                table: "Institution",
                columns: new[] { "UserId", "Nickname" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Institution");
        }
    }
}
