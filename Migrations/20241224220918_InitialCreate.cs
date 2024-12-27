using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace File_Processor.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    category = table.Column<string>(type: "TEXT", nullable: false),
                    filePath = table.Column<string>(type: "TEXT", nullable: false),
                    parentCategory = table.Column<string>(type: "TEXT", nullable: true),
                    subCategory = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.category);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
