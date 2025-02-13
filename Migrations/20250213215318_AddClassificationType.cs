using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace File_Processor.Migrations
{
    /// <inheritdoc />
    public partial class AddClassificationType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "type",
                table: "CategoriesClassification",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "type",
                table: "CategoriesClassification");
        }
    }
}
