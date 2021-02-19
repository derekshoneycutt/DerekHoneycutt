using Microsoft.EntityFrameworkCore.Migrations;

namespace DerekHoneycutt.Migrations
{
    public partial class AddGitHubFieldsToResumeHeadPage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GitHub",
                table: "ResumeHeadPages");

            migrationBuilder.DropColumn(
                name: "GitHubDescription",
                table: "ResumeHeadPages");
        }
    }
}
