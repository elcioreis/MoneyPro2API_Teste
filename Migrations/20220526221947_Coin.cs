using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoneyPro2.Migrations
{
    public partial class Coin : Migration
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

            migrationBuilder.InsertData(
                table: "Coin",
                columns: new[] { "Id", "Active", "Default", "Nickname", "Symbol", "Virtual" },
                values: new object[] { 1, true, true, "Real Brasileiro", "R$", false });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Coin");
        }
    }
}
