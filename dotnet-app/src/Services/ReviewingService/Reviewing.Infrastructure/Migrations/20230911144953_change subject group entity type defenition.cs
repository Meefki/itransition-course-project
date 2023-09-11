using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Reviewing.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class changesubjectgroupentitytypedefenition : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubjectGroups_Reviews_ReviewId",
                schema: "reviewing",
                table: "SubjectGroups");

            migrationBuilder.DropIndex(
                name: "IX_SubjectGroups_ReviewId",
                schema: "reviewing",
                table: "SubjectGroups");

            migrationBuilder.DropColumn(
                name: "ReviewId",
                schema: "reviewing",
                table: "SubjectGroups");

            migrationBuilder.AddColumn<int>(
                name: "Subject_Group",
                schema: "reviewing",
                table: "Reviews",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_Subject_Group",
                schema: "reviewing",
                table: "Reviews",
                column: "Subject_Group");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_SubjectGroups_Subject_Group",
                schema: "reviewing",
                table: "Reviews",
                column: "Subject_Group",
                principalSchema: "reviewing",
                principalTable: "SubjectGroups",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_SubjectGroups_Subject_Group",
                schema: "reviewing",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_Subject_Group",
                schema: "reviewing",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "Subject_Group",
                schema: "reviewing",
                table: "Reviews");

            migrationBuilder.AddColumn<Guid>(
                name: "ReviewId",
                schema: "reviewing",
                table: "SubjectGroups",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_SubjectGroups_ReviewId",
                schema: "reviewing",
                table: "SubjectGroups",
                column: "ReviewId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SubjectGroups_Reviews_ReviewId",
                schema: "reviewing",
                table: "SubjectGroups",
                column: "ReviewId",
                principalSchema: "reviewing",
                principalTable: "Reviews",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
