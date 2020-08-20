using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Guide.Migrations
{
    public partial class addTypeContents : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AskingId",
                table: "QuestionAnswers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResponderId",
                table: "QuestionAnswers",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "State",
                table: "QuestionAnswers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "TypeContents",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(nullable: true),
                    Active = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeContents", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QuestionAnswers_AskingId",
                table: "QuestionAnswers",
                column: "AskingId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionAnswers_ResponderId",
                table: "QuestionAnswers",
                column: "ResponderId");

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionAnswers_AspNetUsers_AskingId",
                table: "QuestionAnswers",
                column: "AskingId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionAnswers_AspNetUsers_ResponderId",
                table: "QuestionAnswers",
                column: "ResponderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuestionAnswers_AspNetUsers_AskingId",
                table: "QuestionAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestionAnswers_AspNetUsers_ResponderId",
                table: "QuestionAnswers");

            migrationBuilder.DropTable(
                name: "TypeContents");

            migrationBuilder.DropIndex(
                name: "IX_QuestionAnswers_AskingId",
                table: "QuestionAnswers");

            migrationBuilder.DropIndex(
                name: "IX_QuestionAnswers_ResponderId",
                table: "QuestionAnswers");

            migrationBuilder.DropColumn(
                name: "AskingId",
                table: "QuestionAnswers");

            migrationBuilder.DropColumn(
                name: "ResponderId",
                table: "QuestionAnswers");

            migrationBuilder.DropColumn(
                name: "State",
                table: "QuestionAnswers");
        }
    }
}
