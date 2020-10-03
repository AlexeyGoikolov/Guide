using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Guide.Migrations
{
    public partial class PostBusinessProcessEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhysicalPath",
                table: "Posts");

            migrationBuilder.AddColumn<string>(
                name: "AdditionalInformation",
                table: "Posts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CoverPath",
                table: "Posts",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PostBusinessProcesses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BusinessProcessId = table.Column<int>(nullable: false),
                    PostId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostBusinessProcesses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PostBusinessProcesses_BusinessProcesses_BusinessProcessId",
                        column: x => x.BusinessProcessId,
                        principalTable: "BusinessProcesses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PostBusinessProcesses_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PostBusinessProcesses_BusinessProcessId",
                table: "PostBusinessProcesses",
                column: "BusinessProcessId");

            migrationBuilder.CreateIndex(
                name: "IX_PostBusinessProcesses_PostId",
                table: "PostBusinessProcesses",
                column: "PostId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PostBusinessProcesses");

            migrationBuilder.DropColumn(
                name: "AdditionalInformation",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "CoverPath",
                table: "Posts");

            migrationBuilder.AddColumn<string>(
                name: "PhysicalPath",
                table: "Posts",
                type: "text",
                nullable: true);
        }
    }
}
