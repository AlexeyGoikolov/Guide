using Microsoft.EntityFrameworkCore.Migrations;

namespace Guide.Migrations
{
    public partial class TypesDataSeeding : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Glossaries",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(25)",
                oldMaxLength: 25);
            
            migrationBuilder.InsertData(
                "Types",
                new[] {"Id", "Name", "Active"},
                new object[]{1, "Книга", true }
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Glossaries",
                type: "character varying(25)",
                maxLength: 25,
                nullable: false,
                oldClrType: typeof(string));
            
            migrationBuilder.DeleteData(
                "Types",
                new[] {"Id", "Name", "Active"},
                new object[]{1, "Книга", true }
            );
        }
    }
}
