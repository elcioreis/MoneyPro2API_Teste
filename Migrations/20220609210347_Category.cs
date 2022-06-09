using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoneyPro2.Migrations
{
    public partial class Category : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "Fk_CategoryGroups_User",
                table: "CategoryGroup");

            migrationBuilder.DropForeignKey(
                name: "Fk_Entry_User",
                table: "Entry");

            migrationBuilder.DropForeignKey(
                name: "Fk_InstitutionTypes_User",
                table: "InstitutionType");

            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "INT", nullable: false),
                    Name = table.Column<string>(type: "VARCHAR(40)", maxLength: 40, nullable: false),
                    Description = table.Column<string>(type: "VARCHAR(150)", maxLength: 150, nullable: false),
                    CategoryParentId = table.Column<int>(type: "INT", nullable: true),
                    CategoryGroupId = table.Column<int>(type: "INT", nullable: true),
                    CrdDeb = table.Column<string>(type: "CHAR(1)", maxLength: 1, nullable: true),
                    VisualOrder = table.Column<int>(type: "INT", nullable: true),
                    Fixed = table.Column<bool>(type: "BIT", nullable: false),
                    System = table.Column<bool>(type: "BIT", nullable: false),
                    Active = table.Column<bool>(type: "BIT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Category_CategoryGroup",
                        column: x => x.CategoryGroupId,
                        principalTable: "CategoryGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Category_CategoryParent",
                        column: x => x.CategoryParentId,
                        principalTable: "Category",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "Fk_Category_User",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Category_CategoryGroupId",
                table: "Category",
                column: "CategoryGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Category_CategoryParentId",
                table: "Category",
                column: "CategoryParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Category_UserId_Name",
                table: "Category",
                columns: new[] { "UserId", "Name" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "Fk_CategoryGroup_User",
                table: "CategoryGroup",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "Fk_Entry_User",
                table: "Entry",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "Fk_InstitutionTypes_User",
                table: "InstitutionType",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "Fk_CategoryGroup_User",
                table: "CategoryGroup");

            migrationBuilder.DropForeignKey(
                name: "Fk_Entry_User",
                table: "Entry");

            migrationBuilder.DropForeignKey(
                name: "Fk_InstitutionTypes_User",
                table: "InstitutionType");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.AddForeignKey(
                name: "Fk_CategoryGroups_User",
                table: "CategoryGroup",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "Fk_Entry_User",
                table: "Entry",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "Fk_InstitutionTypes_User",
                table: "InstitutionType",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
