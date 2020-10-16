using Microsoft.EntityFrameworkCore.Migrations;

namespace Guide.Migrations
{
    public partial class QuestionAnswerStepFKChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuestionAnswers_Sources_SourceId",
                table: "QuestionAnswers");

            migrationBuilder.DropIndex(
                name: "IX_QuestionAnswers_SourceId",
                table: "QuestionAnswers");

            migrationBuilder.DropColumn(
                name: "SourceId",
                table: "QuestionAnswers");

            migrationBuilder.AddColumn<int>(
                name: "StepId",
                table: "QuestionAnswers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_QuestionAnswers_StepId",
                table: "QuestionAnswers",
                column: "StepId");

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionAnswers_Steps_StepId",
                table: "QuestionAnswers",
                column: "StepId",
                principalTable: "Steps",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuestionAnswers_Steps_StepId",
                table: "QuestionAnswers");

            migrationBuilder.DropIndex(
                name: "IX_QuestionAnswers_StepId",
                table: "QuestionAnswers");

            migrationBuilder.DropColumn(
                name: "StepId",
                table: "QuestionAnswers");

            migrationBuilder.AddColumn<int>(
                name: "SourceId",
                table: "QuestionAnswers",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_QuestionAnswers_SourceId",
                table: "QuestionAnswers",
                column: "SourceId");

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionAnswers_Sources_SourceId",
                table: "QuestionAnswers",
                column: "SourceId",
                principalTable: "Sources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
