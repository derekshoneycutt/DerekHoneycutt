using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DerekHoneycutt.Migrations
{
    public partial class AddGitHubPages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GitHub",
                table: "ResumeHeadPages");

            migrationBuilder.DropColumn(
                name: "GitHubDescription",
                table: "ResumeHeadPages");

            migrationBuilder.CreateTable(
                name: "GitHubPage",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    PageId = table.Column<Guid>(type: "TEXT", nullable: false),
                    GitHub = table.Column<string>(type: "TEXT", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GitHubPage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GitHubPage_Pages_PageId",
                        column: x => x.PageId,
                        principalTable: "Pages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GitHubPage_PageId",
                table: "GitHubPage",
                column: "PageId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GitHubPage");

            migrationBuilder.AddColumn<string>(
                name: "GitHub",
                table: "ResumeHeadPages",
                type: "TEXT",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GitHubDescription",
                table: "ResumeHeadPages",
                type: "TEXT",
                maxLength: 2048,
                nullable: true);
        }
    }
}
