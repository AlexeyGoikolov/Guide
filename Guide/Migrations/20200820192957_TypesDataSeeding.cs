using Microsoft.EntityFrameworkCore.Migrations;

namespace Guide.Migrations
{
    public partial class TypesDataSeeding : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                "Types",
                new[] {"Id", "Name", "Active"},
                new object[]{1, "Книга", true }
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                "Types",
                new[] {"Id", "Name", "Active"},
                new object[]{1, "Книга", true }
            );
        }
    }
}
