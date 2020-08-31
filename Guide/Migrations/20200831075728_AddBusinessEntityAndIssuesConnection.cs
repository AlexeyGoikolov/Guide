using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Guide.Migrations
{
    public partial class AddBusinessEntityAndIssuesConnection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BusinessProcesses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessProcesses", x => x.Id);
                });
            

            migrationBuilder.CreateTable(
                name: "BusinessProcessIssues",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BusinessProcessId = table.Column<int>(nullable: false),
                    IssueId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessProcessIssues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BusinessProcessIssues_BusinessProcesses_BusinessProcessId",
                        column: x => x.BusinessProcessId,
                        principalTable: "BusinessProcesses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BusinessProcessIssues_Issues_IssueId",
                        column: x => x.IssueId,
                        principalTable: "Issues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
            
            migrationBuilder.CreateIndex(
                name: "IX_BusinessProcessIssues_BusinessProcessId",
                table: "BusinessProcessIssues",
                column: "BusinessProcessId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessProcessIssues_IssueId",
                table: "BusinessProcessIssues",
                column: "IssueId");
            
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BusinessProcessIssues");

            migrationBuilder.DropTable(
                name: "BusinessProcesses");


        }
    }
}
