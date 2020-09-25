using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Guide.Migrations
{
    public partial class AddBookBusinessProcessAddBookIdAndEnglishBookId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BookBusinessProcesses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BusinessProcessId = table.Column<int>(nullable: false),
                    BookId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookBusinessProcesses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookBusinessProcesses_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookBusinessProcesses_BusinessProcesses_BusinessProcessId",
                        column: x => x.BusinessProcessId,
                        principalTable: "BusinessProcesses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookIdAndEnglishBookIds",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EnglishBookId = table.Column<int>(nullable: false),
                    BookId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookIdAndEnglishBookIds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookIdAndEnglishBookIds_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookIdAndEnglishBookIds_Books_EnglishBookId",
                        column: x => x.EnglishBookId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookBusinessProcesses_BookId",
                table: "BookBusinessProcesses",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_BookBusinessProcesses_BusinessProcessId",
                table: "BookBusinessProcesses",
                column: "BusinessProcessId");

            migrationBuilder.CreateIndex(
                name: "IX_BookIdAndEnglishBookIds_BookId",
                table: "BookIdAndEnglishBookIds",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_BookIdAndEnglishBookIds_EnglishBookId",
                table: "BookIdAndEnglishBookIds",
                column: "EnglishBookId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookBusinessProcesses");

            migrationBuilder.DropTable(
                name: "BookIdAndEnglishBookIds");
        }
    }
}
