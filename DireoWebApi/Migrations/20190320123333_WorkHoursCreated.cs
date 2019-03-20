using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DireoWebApi.Migrations
{
    public partial class WorkHoursCreated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WorkHours",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 50, nullable: false),
                    PlaceId = table.Column<string>(maxLength: 36, nullable: false),
                    Day = table.Column<int>(nullable: false),
                    Open = table.Column<TimeSpan>(type: "time", nullable: false),
                    Close = table.Column<TimeSpan>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkHours", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkHours_Places_PlaceId",
                        column: x => x.PlaceId,
                        principalTable: "Places",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkHours_PlaceId",
                table: "WorkHours",
                column: "PlaceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorkHours");
        }
    }
}
