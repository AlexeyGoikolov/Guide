using Microsoft.EntityFrameworkCore.Migrations;

namespace Guide.Migrations
{
    public partial class deletTaskUser_PostId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskUsers_Posts_PostId",
                table: "TaskUsers");

            migrationBuilder.DropIndex(
                name: "IX_TaskUsers_PostId",
                table: "TaskUsers");

            migrationBuilder.DropColumn(
                name: "PostId",
                table: "TaskUsers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PostId",
                table: "TaskUsers",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TaskUsers_PostId",
                table: "TaskUsers",
                column: "PostId");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskUsers_Posts_PostId",
                table: "TaskUsers",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
