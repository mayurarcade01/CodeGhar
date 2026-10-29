using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodeGhar.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSemanticVersionParts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Major",
                table: "CodeVersions",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Minor",
                table: "CodeVersions",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Major",
                table: "CodeVersions");

            migrationBuilder.DropColumn(
                name: "Minor",
                table: "CodeVersions");
        }
    }
}
