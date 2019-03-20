using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DireoWebApi.Migrations
{
    public partial class PlacesAndCategoryCreated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 36, nullable: false),
                    Name = table.Column<string>(maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Places",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 36, nullable: false),
                    Name = table.Column<string>(maxLength: 150, nullable: true),
                    Ranking = table.Column<decimal>(nullable: false),
                    ReviewCount = table.Column<int>(nullable: false),
                    Desc = table.Column<string>(type: "nvarchar(MAX)", nullable: true),
                    ShortDesc = table.Column<string>(maxLength: 250, nullable: true),
                    Photo = table.Column<string>(maxLength: 100, nullable: true),
                    PhotoFileName = table.Column<string>(maxLength: 100, nullable: true),
                    Video = table.Column<string>(maxLength: 250, nullable: true),
                    Gender = table.Column<bool>(nullable: false),
                    Price = table.Column<decimal>(type: "money", nullable: false),
                    Phone = table.Column<string>(maxLength: 100, nullable: true),
                    CreateAt = table.Column<DateTime>(nullable: true),
                    HideContactInfo = table.Column<bool>(nullable: false),
                    HideMap = table.Column<bool>(nullable: false),
                    Lat = table.Column<string>(maxLength: 100, nullable: true),
                    Long = table.Column<string>(maxLength: 100, nullable: true),
                    Website = table.Column<string>(maxLength: 150, nullable: true),
                    Address = table.Column<string>(maxLength: 150, nullable: true),
                    CategoryId = table.Column<string>(maxLength: 36, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Places", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Places_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Places_CategoryId",
                table: "Places",
                column: "CategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Places");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
