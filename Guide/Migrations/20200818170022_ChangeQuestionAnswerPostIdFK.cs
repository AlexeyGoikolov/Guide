using Microsoft.EntityFrameworkCore.Migrations;

namespace Guide.Migrations
{
    public partial class ChangeQuestionAnswerPostIdFK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuestionAnswers_Posts_PostId",
                table: "QuestionAnswers");

            migrationBuilder.AlterColumn<int>(
                name: "PostId",
                table: "QuestionAnswers",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

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

            migrationBuilder.AlterColumn<int>(
                name: "PostId",
                table: "QuestionAnswers",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionAnswers_Posts_PostId",
                table: "QuestionAnswers",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
