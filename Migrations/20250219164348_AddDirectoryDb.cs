using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace File_Processor.Migrations
{
    /// <inheritdoc />
    public partial class AddDirectoryDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Directories",
                columns: table => new
                {
                    directoryPath = table.Column<string>(type: "TEXT", nullable: false),
                    directoryName = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Directories", x => x.directoryPath);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Directories");
        }
    }
}
