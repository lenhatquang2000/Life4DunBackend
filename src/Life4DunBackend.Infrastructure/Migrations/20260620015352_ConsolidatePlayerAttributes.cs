using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Life4DunBackend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ConsolidatePlayerAttributes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. Add Attributes column as nullable first to allow adding it to tables with existing rows
            migrationBuilder.AddColumn<string>(
                name: "Attributes",
                table: "Players",
                type: "json",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            // 2. Migrate existing data into the JSON column
            migrationBuilder.Sql("UPDATE Players SET Attributes = JSON_OBJECT('Level', Level, 'Attack', Attack, 'Defense', Defense, 'Speed', Speed, 'Health', Health, 'MaxHealth', MaxHealth);");

            // 3. Make Attributes column non-nullable
            migrationBuilder.AlterColumn<string>(
                name: "Attributes",
                table: "Players",
                type: "json",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "json",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            // 4. Drop the old columns
            migrationBuilder.DropColumn(name: "Attack", table: "Players");
            migrationBuilder.DropColumn(name: "Defense", table: "Players");
            migrationBuilder.DropColumn(name: "Health", table: "Players");
            migrationBuilder.DropColumn(name: "Level", table: "Players");
            migrationBuilder.DropColumn(name: "MaxHealth", table: "Players");
            migrationBuilder.DropColumn(name: "Speed", table: "Players");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // 1. Add back the old columns
            migrationBuilder.AddColumn<int>(
                name: "Attack",
                table: "Players",
                type: "int",
                nullable: false,
                defaultValue: 10);

            migrationBuilder.AddColumn<int>(
                name: "Defense",
                table: "Players",
                type: "int",
                nullable: false,
                defaultValue: 5);

            migrationBuilder.AddColumn<int>(
                name: "Health",
                table: "Players",
                type: "int",
                nullable: false,
                defaultValue: 100);

            migrationBuilder.AddColumn<int>(
                name: "Level",
                table: "Players",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "MaxHealth",
                table: "Players",
                type: "int",
                nullable: false,
                defaultValue: 100);

            migrationBuilder.AddColumn<int>(
                name: "Speed",
                table: "Players",
                type: "int",
                nullable: false,
                defaultValue: 10);

            // 2. Migrate data from JSON back to columns
            migrationBuilder.Sql(@"
                UPDATE Players SET 
                    Level = COALESCE(CAST(JSON_EXTRACT(Attributes, '$.Level') AS SIGNED), 1),
                    Attack = COALESCE(CAST(JSON_EXTRACT(Attributes, '$.Attack') AS SIGNED), 10),
                    Defense = COALESCE(CAST(JSON_EXTRACT(Attributes, '$.Defense') AS SIGNED), 5),
                    Speed = COALESCE(CAST(JSON_EXTRACT(Attributes, '$.Speed') AS SIGNED), 10),
                    Health = COALESCE(CAST(JSON_EXTRACT(Attributes, '$.Health') AS SIGNED), 100),
                    MaxHealth = COALESCE(CAST(JSON_EXTRACT(Attributes, '$.MaxHealth') AS SIGNED), 100);
            ");

            // 3. Drop JSON column
            migrationBuilder.DropColumn(
                name: "Attributes",
                table: "Players");
        }
    }
}
