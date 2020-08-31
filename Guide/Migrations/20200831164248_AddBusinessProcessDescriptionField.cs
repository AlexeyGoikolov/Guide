using Microsoft.EntityFrameworkCore.Migrations;

namespace Guide.Migrations
{
    public partial class AddBusinessProcessDescriptionField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "BusinessProcesses",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "BusinessProcesses");
        }
    }
}
