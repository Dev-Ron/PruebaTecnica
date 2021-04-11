using Microsoft.EntityFrameworkCore.Migrations;

namespace Infracstructure.Migrations
{
    public partial class IdsApi : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdApi",
                table: "Libro",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdApi",
                table: "Autor",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdApi",
                table: "Libro");

            migrationBuilder.DropColumn(
                name: "IdApi",
                table: "Autor");
        }
    }
}
