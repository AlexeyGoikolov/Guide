using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Guide.Migrations
{
    public partial class DesiredResult : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DesiredResults",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Number = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DesiredResults", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DesiredResultIssue",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IssueId = table.Column<int>(nullable: false),
                    DesiredResultId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DesiredResultIssue", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DesiredResultIssue_DesiredResults_DesiredResultId",
                        column: x => x.DesiredResultId,
                        principalTable: "DesiredResults",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DesiredResultIssue_Issues_IssueId",
                        column: x => x.IssueId,
                        principalTable: "Issues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DesiredResultIssue_DesiredResultId",
                table: "DesiredResultIssue",
                column: "DesiredResultId");

            migrationBuilder.CreateIndex(
                name: "IX_DesiredResultIssue_IssueId",
                table: "DesiredResultIssue",
                column: "IssueId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DesiredResultIssue");

            migrationBuilder.DropTable(
                name: "DesiredResults");
        }
    }
}
