using Microsoft.EntityFrameworkCore.Migrations;

namespace Guide.Migrations
{
    public partial class addTemplate_Title : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Templates",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "Templates");
        }
    }
}
