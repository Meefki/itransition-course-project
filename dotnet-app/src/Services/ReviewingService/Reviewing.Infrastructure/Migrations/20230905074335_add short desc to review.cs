using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Reviewing.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addshortdesctoreview : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ShortDesc",
                schema: "reviewing",
                table: "Reviews",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShortDesc",
                schema: "reviewing",
                table: "Reviews");
        }
    }
}
