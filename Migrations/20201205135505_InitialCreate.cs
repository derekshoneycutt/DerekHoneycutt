using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DerekHoneycutt.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Landings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Href = table.Column<string>(type: "TEXT", maxLength: 350, nullable: true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Subtitle = table.Column<string>(type: "TEXT", maxLength: 350, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Landings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    LandingId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Type = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Title = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    Subtitle = table.Column<string>(type: "TEXT", maxLength: 350, nullable: true),
                    Background = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    Image = table.Column<string>(type: "TEXT", maxLength: 350, nullable: true),
                    Orientation = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pages_Landings_LandingId",
                        column: x => x.LandingId,
                        principalTable: "Landings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ImageWallPages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    PageId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 2048, nullable: true),
                    Images = table.Column<string>(type: "TEXT", maxLength: 2048, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImageWallPages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ImageWallPages_Pages_PageId",
                        column: x => x.PageId,
                        principalTable: "Pages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ResumeExpPages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    PageId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResumeExpPages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResumeExpPages_Pages_PageId",
                        column: x => x.PageId,
                        principalTable: "Pages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ResumeHeadPages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    PageId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 2048, nullable: true),
                    Competencies = table.Column<string>(type: "TEXT", maxLength: 2048, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResumeHeadPages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResumeHeadPages_Pages_PageId",
                        column: x => x.PageId,
                        principalTable: "Pages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SchoolsPages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    PageId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchoolsPages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SchoolsPages_Pages_PageId",
                        column: x => x.PageId,
                        principalTable: "Pages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TextBlockPages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    PageId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Text = table.Column<string>(type: "TEXT", maxLength: 2048, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TextBlockPages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TextBlockPages_Pages_PageId",
                        column: x => x.PageId,
                        principalTable: "Pages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ResumeExpJobs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    PageId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Employer = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    EmployerCity = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    StartDate = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    EndDate = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "TEXT", maxLength: 2048, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResumeExpJobs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResumeExpJobs_ResumeExpPages_PageId",
                        column: x => x.PageId,
                        principalTable: "ResumeExpPages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Schools",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    PageId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    City = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    StartDate = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    EndDate = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    Program = table.Column<string>(type: "TEXT", maxLength: 350, nullable: true),
                    GPA = table.Column<decimal>(type: "decimal(8,6)", nullable: true),
                    Other = table.Column<string>(type: "TEXT", maxLength: 2048, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schools", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Schools_SchoolsPages_PageId",
                        column: x => x.PageId,
                        principalTable: "SchoolsPages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ImageWallPages_PageId",
                table: "ImageWallPages",
                column: "PageId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pages_LandingId",
                table: "Pages",
                column: "LandingId");

            migrationBuilder.CreateIndex(
                name: "IX_ResumeExpJobs_PageId",
                table: "ResumeExpJobs",
                column: "PageId");

            migrationBuilder.CreateIndex(
                name: "IX_ResumeExpPages_PageId",
                table: "ResumeExpPages",
                column: "PageId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ResumeHeadPages_PageId",
                table: "ResumeHeadPages",
                column: "PageId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Schools_PageId",
                table: "Schools",
                column: "PageId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolsPages_PageId",
                table: "SchoolsPages",
                column: "PageId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TextBlockPages_PageId",
                table: "TextBlockPages",
                column: "PageId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ImageWallPages");

            migrationBuilder.DropTable(
                name: "ResumeExpJobs");

            migrationBuilder.DropTable(
                name: "ResumeHeadPages");

            migrationBuilder.DropTable(
                name: "Schools");

            migrationBuilder.DropTable(
                name: "TextBlockPages");

            migrationBuilder.DropTable(
                name: "ResumeExpPages");

            migrationBuilder.DropTable(
                name: "SchoolsPages");

            migrationBuilder.DropTable(
                name: "Pages");

            migrationBuilder.DropTable(
                name: "Landings");
        }
    }
}
