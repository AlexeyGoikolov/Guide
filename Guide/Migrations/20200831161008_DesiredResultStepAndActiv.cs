using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Guide.Migrations
{
    public partial class DesiredResultStepAndActiv : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "DesiredResults",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "DesiredResultIssue",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "DesiredResultStep",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StepId = table.Column<int>(nullable: false),
                    DesiredResultId = table.Column<int>(nullable: false),
                    Active = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DesiredResultStep", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DesiredResultStep_DesiredResults_DesiredResultId",
                        column: x => x.DesiredResultId,
                        principalTable: "DesiredResults",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DesiredResultStep_Steps_StepId",
                        column: x => x.StepId,
                        principalTable: "Steps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DesiredResultStep_DesiredResultId",
                table: "DesiredResultStep",
                column: "DesiredResultId");

            migrationBuilder.CreateIndex(
                name: "IX_DesiredResultStep_StepId",
                table: "DesiredResultStep",
                column: "StepId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DesiredResultStep");

            migrationBuilder.DropColumn(
                name: "Active",
                table: "DesiredResults");

            migrationBuilder.DropColumn(
                name: "Active",
                table: "DesiredResultIssue");
        }
    }
}
