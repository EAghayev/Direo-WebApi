using Microsoft.EntityFrameworkCore.Migrations;

namespace DireoWebApi.Migrations
{
    public partial class FaqsCreated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PlaceFaqs",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 36, nullable: false),
                    Question = table.Column<string>(maxLength: 500, nullable: false),
                    Answer = table.Column<string>(maxLength: 500, nullable: false),
                    PlaceId = table.Column<string>(maxLength: 36, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlaceFaqs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlaceFaqs_Places_PlaceId",
                        column: x => x.PlaceId,
                        principalTable: "Places",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlaceFaqs_PlaceId",
                table: "PlaceFaqs",
                column: "PlaceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlaceFaqs");
        }
    }
}
