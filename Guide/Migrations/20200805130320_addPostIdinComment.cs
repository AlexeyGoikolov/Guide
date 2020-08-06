using Microsoft.EntityFrameworkCore.Migrations;

namespace Guide.Migrations
{
    public partial class addPostIdinComment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PostId",
                table: "QuestionAnswers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_QuestionAnswers_PostId",
                table: "QuestionAnswers",
                column: "PostId");

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionAnswers_Posts_PostId",
                table: "QuestionAnswers",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuestionAnswers_Posts_PostId",
                table: "QuestionAnswers");

            migrationBuilder.DropIndex(
                name: "IX_QuestionAnswers_PostId",
                table: "QuestionAnswers");

            migrationBuilder.DropColumn(
                name: "PostId",
                table: "QuestionAnswers");
        }
    }
}
