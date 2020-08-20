using Microsoft.EntityFrameworkCore.Migrations;

namespace Guide.Migrations
{
    public partial class addPost_UserId_SId_CId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TypeContentId",
                table: "Posts",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TypeStateId",
                table: "Posts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Posts",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Posts_TypeContentId",
                table: "Posts",
                column: "TypeContentId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_TypeStateId",
                table: "Posts",
                column: "TypeStateId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_UserId",
                table: "Posts",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_TypeContents_TypeContentId",
                table: "Posts",
                column: "TypeContentId",
                principalTable: "TypeContents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_TypeStates_TypeStateId",
                table: "Posts",
                column: "TypeStateId",
                principalTable: "TypeStates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_AspNetUsers_UserId",
                table: "Posts",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_TypeContents_TypeContentId",
                table: "Posts");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_TypeStates_TypeStateId",
                table: "Posts");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_AspNetUsers_UserId",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_TypeContentId",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_TypeStateId",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_UserId",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "TypeContentId",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "TypeStateId",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Posts");
        }
    }
}
