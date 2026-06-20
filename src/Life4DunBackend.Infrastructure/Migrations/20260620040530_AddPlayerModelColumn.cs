using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Life4DunBackend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPlayerModelColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Model",
                table: "Players",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "Mira")
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Model",
                table: "Players");
        }
    }
}
