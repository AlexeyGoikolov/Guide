using Microsoft.EntityFrameworkCore.Migrations;

namespace Guide.Migrations
{
    public partial class Glossary_addAbbreviation_addSource : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Abbreviation",
                table: "Glossaries",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Source",
                table: "Glossaries",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Abbreviation",
                table: "Glossaries");

            migrationBuilder.DropColumn(
                name: "Source",
                table: "Glossaries");
        }
    }
}
