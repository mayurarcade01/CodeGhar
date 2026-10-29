using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodeGhar.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCodeGharSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CodeFiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 180, nullable: false),
                    Extension = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CurrentVersionNumber = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CodeFiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CodeVersions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    CodeFileId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Number = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Message = table.Column<string>(type: "TEXT", maxLength: 160, nullable: true),
                    Content = table.Column<string>(type: "TEXT", nullable: false),
                    AddedLines = table.Column<int>(type: "INTEGER", nullable: false),
                    DeletedLines = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CodeVersions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CodeVersions_CodeFiles_CodeFileId",
                        column: x => x.CodeFileId,
                        principalTable: "CodeFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CodeFiles_Name_IsDeleted",
                table: "CodeFiles",
                columns: new[] { "Name", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_CodeVersions_CodeFileId_Number",
                table: "CodeVersions",
                columns: new[] { "CodeFileId", "Number" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CodeVersions");

            migrationBuilder.DropTable(
                name: "CodeFiles");
        }
    }
}
