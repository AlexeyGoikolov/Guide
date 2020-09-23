using Microsoft.EntityFrameworkCore.Migrations;

namespace Guide.Migrations
{
    public partial class AddAuthorAndAddBookAuthor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Author",
                table: "Books");

            migrationBuilder.CreateIndex(
                name: "IX_Glossaries_GlossarysId",
                table: "Glossaries",
                column: "GlossarysId");

            migrationBuilder.AddForeignKey(
                name: "FK_Glossaries_Glossaries_GlossarysId",
                table: "Glossaries",
                column: "GlossarysId",
                principalTable: "Glossaries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Glossaries_Glossaries_GlossarysId",
                table: "Glossaries");

            migrationBuilder.DropIndex(
                name: "IX_Glossaries_GlossarysId",
                table: "Glossaries");

            migrationBuilder.AddColumn<string>(
                name: "Author",
                table: "Books",
                type: "text",
                nullable: true);
        }
    }
}
