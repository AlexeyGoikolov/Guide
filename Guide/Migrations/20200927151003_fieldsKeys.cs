using Microsoft.EntityFrameworkCore.Migrations;

namespace Guide.Migrations
{
    public partial class fieldsKeys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Keys",
                table: "Posts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Keys",
                table: "Books",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Keys",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "Keys",
                table: "Books");
        }
    }
}
