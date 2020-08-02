using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Guide.Migrations
{
    public partial class AddBookFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateOfWriting",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Books");

            migrationBuilder.AlterColumn<string>(
                name: "Question",
                table: "QuestionAnswers",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Books",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TypeId",
                table: "Books",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "YearOfWriting",
                table: "Books",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Books_TypeId",
                table: "Books",
                column: "TypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Types_TypeId",
                table: "Books",
                column: "TypeId",
                principalTable: "Types",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Types_TypeId",
                table: "Books");

            migrationBuilder.DropIndex(
                name: "IX_Books_TypeId",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "TypeId",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "YearOfWriting",
                table: "Books");

            migrationBuilder.AlterColumn<string>(
                name: "Question",
                table: "QuestionAnswers",
                type: "text",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfWriting",
                table: "Books",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Books",
                type: "text",
                nullable: true);
        }
    }
}
