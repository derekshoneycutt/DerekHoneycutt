using Microsoft.EntityFrameworkCore.Migrations;

namespace DerekHoneycutt.Migrations
{
    public partial class OrderSchools : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "Schools",
                type: "integer",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Order",
                table: "Schools");
        }
    }
}
