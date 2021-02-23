using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DerekHoneycutt.Migrations
{
    public partial class ResetMigrations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Landings",
                columns: table => new
                {
                    Index = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Href = table.Column<string>(type: "nvarchar(350)", maxLength: 350, nullable: true),
                    Title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Subtitle = table.Column<string>(type: "nvarchar(350)", maxLength: 350, nullable: true),
                    Icon = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Order = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Landings", x => x.Index);
                    table.UniqueConstraint("AK_Landings_Id", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pages",
                columns: table => new
                {
                    Index = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LandingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Subtitle = table.Column<string>(type: "nvarchar(350)", maxLength: 350, nullable: true),
                    Background = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Image = table.Column<string>(type: "nvarchar(350)", maxLength: 350, nullable: true),
                    Orientation = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pages", x => x.Index);
                    table.UniqueConstraint("AK_Pages_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pages_Landings_LandingId",
                        column: x => x.LandingId,
                        principalTable: "Landings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GitHubPage",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Index = table.Column<int>(type: "int", nullable: true),
                    PageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GitHub = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
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

            migrationBuilder.CreateTable(
                name: "ImageWallPages",
                columns: table => new
                {
                    Index = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImageWallPages", x => x.Index);
                    table.UniqueConstraint("AK_ImageWallPages_Id", x => x.Id);
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
                    Index = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResumeExpPages", x => x.Index);
                    table.UniqueConstraint("AK_ResumeExpPages_Id", x => x.Id);
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
                    Index = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true),
                    Competencies = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResumeHeadPages", x => x.Index);
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
                    Index = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchoolsPages", x => x.Index);
                    table.UniqueConstraint("AK_SchoolsPages_Id", x => x.Id);
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
                    Index = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TextBlockPages", x => x.Index);
                    table.ForeignKey(
                        name: "FK_TextBlockPages_Pages_PageId",
                        column: x => x.PageId,
                        principalTable: "Pages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    Index = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Source = table.Column<string>(type: "nvarchar(350)", maxLength: 350, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true),
                    Order = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.Index);
                    table.ForeignKey(
                        name: "FK_Images_ImageWallPages_PageId",
                        column: x => x.PageId,
                        principalTable: "ImageWallPages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ResumeExpJobs",
                columns: table => new
                {
                    Index = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Employer = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    EmployerCity = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    StartDate = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    EndDate = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResumeExpJobs", x => x.Index);
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
                    Index = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    City = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    StartDate = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    EndDate = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Program = table.Column<string>(type: "nvarchar(350)", maxLength: 350, nullable: true),
                    GPA = table.Column<decimal>(type: "decimal(8,6)", nullable: true),
                    Other = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true),
                    Order = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schools", x => x.Index);
                    table.ForeignKey(
                        name: "FK_Schools_SchoolsPages_PageId",
                        column: x => x.PageId,
                        principalTable: "SchoolsPages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GitHubPage_PageId",
                table: "GitHubPage",
                column: "PageId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Images_Id",
                table: "Images",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Images_PageId",
                table: "Images",
                column: "PageId");

            migrationBuilder.CreateIndex(
                name: "IX_ImageWallPages_Id",
                table: "ImageWallPages",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ImageWallPages_PageId",
                table: "ImageWallPages",
                column: "PageId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Landings_Id",
                table: "Landings",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pages_Id",
                table: "Pages",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pages_LandingId",
                table: "Pages",
                column: "LandingId");

            migrationBuilder.CreateIndex(
                name: "IX_ResumeExpJobs_Id",
                table: "ResumeExpJobs",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ResumeExpJobs_PageId",
                table: "ResumeExpJobs",
                column: "PageId");

            migrationBuilder.CreateIndex(
                name: "IX_ResumeExpPages_Id",
                table: "ResumeExpPages",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ResumeExpPages_PageId",
                table: "ResumeExpPages",
                column: "PageId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ResumeHeadPages_Id",
                table: "ResumeHeadPages",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ResumeHeadPages_PageId",
                table: "ResumeHeadPages",
                column: "PageId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Schools_Id",
                table: "Schools",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Schools_PageId",
                table: "Schools",
                column: "PageId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolsPages_Id",
                table: "SchoolsPages",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SchoolsPages_PageId",
                table: "SchoolsPages",
                column: "PageId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TextBlockPages_Id",
                table: "TextBlockPages",
                column: "Id",
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
                name: "GitHubPage");

            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.DropTable(
                name: "ResumeExpJobs");

            migrationBuilder.DropTable(
                name: "ResumeHeadPages");

            migrationBuilder.DropTable(
                name: "Schools");

            migrationBuilder.DropTable(
                name: "TextBlockPages");

            migrationBuilder.DropTable(
                name: "ImageWallPages");

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
