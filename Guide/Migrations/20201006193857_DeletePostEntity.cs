using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Guide.Migrations
{
    public partial class DeletePostEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Authors_Books_BookId",
                table: "Authors");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Books_BookId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Posts_PostId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestionAnswers_Posts_PostId",
                table: "QuestionAnswers");

            migrationBuilder.DropTable(
                name: "BookAuthors");

            migrationBuilder.DropTable(
                name: "BookBusinessProcesses");

            migrationBuilder.DropTable(
                name: "BookIdAndEnglishBookIds");

            migrationBuilder.DropTable(
                name: "PostBusinessProcesses");

            migrationBuilder.DropTable(
                name: "Templates");

            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.DropTable(
                name: "Posts");

            migrationBuilder.DropTable(
                name: "TypeContents");

            migrationBuilder.DropTable(
                name: "Types");

            migrationBuilder.DropTable(
                name: "TypeStates");

            migrationBuilder.DropIndex(
                name: "IX_QuestionAnswers_PostId",
                table: "QuestionAnswers");

            migrationBuilder.DropIndex(
                name: "IX_Comments_BookId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_PostId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Authors_BookId",
                table: "Authors");

            migrationBuilder.DropColumn(
                name: "Author",
                table: "Steps");

            migrationBuilder.DropColumn(
                name: "PostId",
                table: "QuestionAnswers");

            migrationBuilder.DropColumn(
                name: "BookId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "PostId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "BookId",
                table: "Authors");

            migrationBuilder.AddColumn<int>(
                name: "SourceId",
                table: "QuestionAnswers",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SourceId",
                table: "Comments",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SourceStates",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(nullable: true),
                    Active = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SourceStates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SourceTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(nullable: true),
                    Active = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SourceTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sources",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(nullable: true),
                    ISBN = table.Column<string>(nullable: true),
                    SourceDescription = table.Column<string>(nullable: true),
                    CoverPath = table.Column<string>(nullable: true),
                    Edition = table.Column<string>(nullable: true),
                    VirtualPath = table.Column<string>(nullable: true),
                    PhysicalPath = table.Column<string>(nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    IsRecipe = table.Column<bool>(nullable: false),
                    SourceTypeId = table.Column<int>(nullable: true),
                    SourceStateId = table.Column<int>(nullable: true),
                    YearOfWriting = table.Column<string>(nullable: true),
                    DateCreate = table.Column<DateTime>(nullable: false),
                    DateUpdate = table.Column<DateTime>(nullable: false),
                    CategoryId = table.Column<int>(nullable: true),
                    TranslationID = table.Column<int>(nullable: false),
                    Keys = table.Column<string>(nullable: true),
                    AdditionalInformation = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sources", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sources_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sources_SourceStates_SourceStateId",
                        column: x => x.SourceStateId,
                        principalTable: "SourceStates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sources_SourceTypes_SourceTypeId",
                        column: x => x.SourceTypeId,
                        principalTable: "SourceTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SourceAuthors",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AuthorId = table.Column<int>(nullable: false),
                    SourceId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SourceAuthors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SourceAuthors_Authors_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Authors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SourceAuthors_Sources_SourceId",
                        column: x => x.SourceId,
                        principalTable: "Sources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SourceBusinessProcesses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BusinessProcessId = table.Column<int>(nullable: false),
                    SourceId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SourceBusinessProcesses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SourceBusinessProcesses_BusinessProcesses_BusinessProcessId",
                        column: x => x.BusinessProcessId,
                        principalTable: "BusinessProcesses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SourceBusinessProcesses_Sources_SourceId",
                        column: x => x.SourceId,
                        principalTable: "Sources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SourceIdAndEnglishSourceIds",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EnglishSourceId = table.Column<int>(nullable: false),
                    SourceId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SourceIdAndEnglishSourceIds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SourceIdAndEnglishSourceIds_Sources_EnglishSourceId",
                        column: x => x.EnglishSourceId,
                        principalTable: "Sources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SourceIdAndEnglishSourceIds_Sources_SourceId",
                        column: x => x.SourceId,
                        principalTable: "Sources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "SourceTypes",
                columns: new[] { "Id", "Active", "Name" },
                values: new object[] { 1, true, "Книга" });

            migrationBuilder.CreateIndex(
                name: "IX_QuestionAnswers_SourceId",
                table: "QuestionAnswers",
                column: "SourceId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_SourceId",
                table: "Comments",
                column: "SourceId");

            migrationBuilder.CreateIndex(
                name: "IX_SourceAuthors_AuthorId",
                table: "SourceAuthors",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_SourceAuthors_SourceId",
                table: "SourceAuthors",
                column: "SourceId");

            migrationBuilder.CreateIndex(
                name: "IX_SourceBusinessProcesses_BusinessProcessId",
                table: "SourceBusinessProcesses",
                column: "BusinessProcessId");

            migrationBuilder.CreateIndex(
                name: "IX_SourceBusinessProcesses_SourceId",
                table: "SourceBusinessProcesses",
                column: "SourceId");

            migrationBuilder.CreateIndex(
                name: "IX_SourceIdAndEnglishSourceIds_EnglishSourceId",
                table: "SourceIdAndEnglishSourceIds",
                column: "EnglishSourceId");

            migrationBuilder.CreateIndex(
                name: "IX_SourceIdAndEnglishSourceIds_SourceId",
                table: "SourceIdAndEnglishSourceIds",
                column: "SourceId");

            migrationBuilder.CreateIndex(
                name: "IX_Sources_CategoryId",
                table: "Sources",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Sources_SourceStateId",
                table: "Sources",
                column: "SourceStateId");

            migrationBuilder.CreateIndex(
                name: "IX_Sources_SourceTypeId",
                table: "Sources",
                column: "SourceTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Sources_SourceId",
                table: "Comments",
                column: "SourceId",
                principalTable: "Sources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionAnswers_Sources_SourceId",
                table: "QuestionAnswers",
                column: "SourceId",
                principalTable: "Sources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Sources_SourceId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestionAnswers_Sources_SourceId",
                table: "QuestionAnswers");

            migrationBuilder.DropTable(
                name: "SourceAuthors");

            migrationBuilder.DropTable(
                name: "SourceBusinessProcesses");

            migrationBuilder.DropTable(
                name: "SourceIdAndEnglishSourceIds");

            migrationBuilder.DropTable(
                name: "Sources");

            migrationBuilder.DropTable(
                name: "SourceStates");

            migrationBuilder.DropTable(
                name: "SourceTypes");

            migrationBuilder.DropIndex(
                name: "IX_QuestionAnswers_SourceId",
                table: "QuestionAnswers");

            migrationBuilder.DropIndex(
                name: "IX_Comments_SourceId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "SourceId",
                table: "QuestionAnswers");

            migrationBuilder.DropColumn(
                name: "SourceId",
                table: "Comments");

            migrationBuilder.AddColumn<string>(
                name: "Author",
                table: "Steps",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PostId",
                table: "QuestionAnswers",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BookId",
                table: "Comments",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PostId",
                table: "Comments",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BookId",
                table: "Authors",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Templates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ContentTemplate = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Title = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Templates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TypeContents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Active = table.Column<bool>(type: "boolean", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeContents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Types",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Active = table.Column<bool>(type: "boolean", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Types", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TypeStates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Active = table.Column<bool>(type: "boolean", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeStates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Active = table.Column<bool>(type: "boolean", nullable: false),
                    CoverPath = table.Column<string>(type: "text", nullable: true),
                    DateCreate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DateUpdate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Edition = table.Column<string>(type: "text", nullable: true),
                    ISBN = table.Column<string>(type: "text", nullable: true),
                    IsRecipe = table.Column<bool>(type: "boolean", nullable: false),
                    Keys = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    PhysicalPath = table.Column<string>(type: "text", nullable: true),
                    TypeId = table.Column<int>(type: "integer", nullable: true),
                    VirtualPath = table.Column<string>(type: "text", nullable: true),
                    YearOfWriting = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Books_Types_TypeId",
                        column: x => x.TypeId,
                        principalTable: "Types",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Active = table.Column<bool>(type: "boolean", nullable: false),
                    AdditionalInformation = table.Column<string>(type: "text", nullable: true),
                    Author = table.Column<string>(type: "text", nullable: true),
                    CategoryId = table.Column<int>(type: "integer", nullable: true),
                    CoverPath = table.Column<string>(type: "text", nullable: true),
                    DateOfCreate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DateOfUpdate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Keys = table.Column<string>(type: "text", nullable: true),
                    TextContent = table.Column<string>(type: "text", nullable: true),
                    Title = table.Column<string>(type: "text", nullable: true),
                    TypeContentId = table.Column<int>(type: "integer", nullable: true),
                    TypeId = table.Column<int>(type: "integer", nullable: true),
                    TypeStateId = table.Column<int>(type: "integer", nullable: true),
                    UserId = table.Column<string>(type: "text", nullable: true),
                    VirtualPath = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Posts_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Posts_TypeContents_TypeContentId",
                        column: x => x.TypeContentId,
                        principalTable: "TypeContents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Posts_Types_TypeId",
                        column: x => x.TypeId,
                        principalTable: "Types",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Posts_TypeStates_TypeStateId",
                        column: x => x.TypeStateId,
                        principalTable: "TypeStates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Posts_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BookAuthors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AuthorId = table.Column<int>(type: "integer", nullable: false),
                    BookId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookAuthors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookAuthors_Authors_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Authors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookAuthors_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookBusinessProcesses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BookId = table.Column<int>(type: "integer", nullable: false),
                    BusinessProcessId = table.Column<int>(type: "integer", nullable: false)
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
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BookId = table.Column<int>(type: "integer", nullable: false),
                    EnglishBookId = table.Column<int>(type: "integer", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "PostBusinessProcesses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BusinessProcessId = table.Column<int>(type: "integer", nullable: false),
                    PostId = table.Column<int>(type: "integer", nullable: false)
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

            migrationBuilder.InsertData(
                table: "Types",
                columns: new[] { "Id", "Active", "Name" },
                values: new object[] { 1, true, "Книга" });

            migrationBuilder.CreateIndex(
                name: "IX_QuestionAnswers_PostId",
                table: "QuestionAnswers",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_BookId",
                table: "Comments",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_PostId",
                table: "Comments",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_Authors_BookId",
                table: "Authors",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_BookAuthors_AuthorId",
                table: "BookAuthors",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_BookAuthors_BookId",
                table: "BookAuthors",
                column: "BookId");

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

            migrationBuilder.CreateIndex(
                name: "IX_Books_TypeId",
                table: "Books",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PostBusinessProcesses_BusinessProcessId",
                table: "PostBusinessProcesses",
                column: "BusinessProcessId");

            migrationBuilder.CreateIndex(
                name: "IX_PostBusinessProcesses_PostId",
                table: "PostBusinessProcesses",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_CategoryId",
                table: "Posts",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_TypeContentId",
                table: "Posts",
                column: "TypeContentId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_TypeId",
                table: "Posts",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_TypeStateId",
                table: "Posts",
                column: "TypeStateId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_UserId",
                table: "Posts",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Authors_Books_BookId",
                table: "Authors",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Books_BookId",
                table: "Comments",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Posts_PostId",
                table: "Comments",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionAnswers_Posts_PostId",
                table: "QuestionAnswers",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
