using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Reviewing.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "reviewing");

            migrationBuilder.CreateTable(
                name: "Comments",
                schema: "reviewing",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReviewId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SentDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SubjectGroups",
                schema: "reviewing",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubjectGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                schema: "reviewing",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "Subjects",
                schema: "reviewing",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GroupId = table.Column<int>(type: "int", nullable: false),
                    Grade = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subjects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subjects_SubjectGroups_GroupId",
                        column: x => x.GroupId,
                        principalSchema: "reviewing",
                        principalTable: "SubjectGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reviews",
                schema: "reviewing",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AuthorUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ShortDesc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PublishedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reviews_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalSchema: "reviewing",
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Estimates",
                schema: "reviewing",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReviewId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Grade = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Estimates", x => new { x.ReviewId, x.UserId });
                    table.ForeignKey(
                        name: "FK_Estimates_Reviews_ReviewId",
                        column: x => x.ReviewId,
                        principalSchema: "reviewing",
                        principalTable: "Reviews",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReviewComments",
                schema: "reviewing",
                columns: table => new
                {
                    CommentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReviewId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReviewComments", x => new { x.CommentId, x.ReviewId });
                    table.ForeignKey(
                        name: "FK_ReviewComments_Reviews_ReviewId",
                        column: x => x.ReviewId,
                        principalSchema: "reviewing",
                        principalTable: "Reviews",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReviewLikes",
                schema: "reviewing",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReviewId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReviewLikes", x => new { x.UserId, x.ReviewId });
                    table.ForeignKey(
                        name: "FK_ReviewLikes_Reviews_ReviewId",
                        column: x => x.ReviewId,
                        principalSchema: "reviewing",
                        principalTable: "Reviews",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReviewTag",
                schema: "reviewing",
                columns: table => new
                {
                    ReviewId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TagsName = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReviewTag", x => new { x.ReviewId, x.TagsName });
                    table.ForeignKey(
                        name: "FK_ReviewTag_Reviews_ReviewId",
                        column: x => x.ReviewId,
                        principalSchema: "reviewing",
                        principalTable: "Reviews",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReviewTag_Tags_TagsName",
                        column: x => x.TagsName,
                        principalSchema: "reviewing",
                        principalTable: "Tags",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReviewComments_ReviewId",
                schema: "reviewing",
                table: "ReviewComments",
                column: "ReviewId");

            migrationBuilder.CreateIndex(
                name: "IX_ReviewLikes_ReviewId",
                schema: "reviewing",
                table: "ReviewLikes",
                column: "ReviewId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_SubjectId",
                schema: "reviewing",
                table: "Reviews",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ReviewTag_TagsName",
                schema: "reviewing",
                table: "ReviewTag",
                column: "TagsName");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectGroups_Name",
                schema: "reviewing",
                table: "SubjectGroups",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_GroupId",
                schema: "reviewing",
                table: "Subjects",
                column: "GroupId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comments",
                schema: "reviewing");

            migrationBuilder.DropTable(
                name: "Estimates",
                schema: "reviewing");

            migrationBuilder.DropTable(
                name: "ReviewComments",
                schema: "reviewing");

            migrationBuilder.DropTable(
                name: "ReviewLikes",
                schema: "reviewing");

            migrationBuilder.DropTable(
                name: "ReviewTag",
                schema: "reviewing");

            migrationBuilder.DropTable(
                name: "Reviews",
                schema: "reviewing");

            migrationBuilder.DropTable(
                name: "Tags",
                schema: "reviewing");

            migrationBuilder.DropTable(
                name: "Subjects",
                schema: "reviewing");

            migrationBuilder.DropTable(
                name: "SubjectGroups",
                schema: "reviewing");
        }
    }
}
