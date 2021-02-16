using Microsoft.EntityFrameworkCore.Migrations;

namespace DerekHoneycutt.Migrations
{
    public partial class LandingIcon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Icon",
                table: "Landings",
                type: "TEXT",
                maxLength: 150,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Icon",
                table: "Landings");
        }
    }
}
