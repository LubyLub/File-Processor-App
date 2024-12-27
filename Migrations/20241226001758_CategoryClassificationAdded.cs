using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace File_Processor.Migrations
{
    /// <inheritdoc />
    public partial class CategoryClassificationAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CategoriesClassification",
                columns: table => new
                {
                    category = table.Column<string>(type: "TEXT", nullable: false),
                    pattern = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoriesClassification", x => new { x.category, x.pattern });
                    table.ForeignKey(
                        name: "FK_CategoriesClassification_Categories_category",
                        column: x => x.category,
                        principalTable: "Categories",
                        principalColumn: "category",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategoriesClassification");
        }
    }
}
