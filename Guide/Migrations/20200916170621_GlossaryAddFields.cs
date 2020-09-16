using Microsoft.EntityFrameworkCore.Migrations;

namespace Guide.Migrations
{
    public partial class GlossaryAddFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GlossarysId",
                table: "Glossaries",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Language",
                table: "Glossaries",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GlossarysId",
                table: "Glossaries");

            migrationBuilder.DropColumn(
                name: "Language",
                table: "Glossaries");
        }
    }
}
