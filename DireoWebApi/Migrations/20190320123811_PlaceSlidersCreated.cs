using Microsoft.EntityFrameworkCore.Migrations;

namespace DireoWebApi.Migrations
{
    public partial class PlaceSlidersCreated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PlaceSliderPhotos",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 36, nullable: false),
                    Photo = table.Column<string>(maxLength: 100, nullable: true),
                    PhotoName = table.Column<string>(maxLength: 100, nullable: true),
                    PlaceId = table.Column<string>(maxLength: 36, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlaceSliderPhotos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlaceSliderPhotos_Places_PlaceId",
                        column: x => x.PlaceId,
                        principalTable: "Places",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlaceSliderPhotos_PlaceId",
                table: "PlaceSliderPhotos",
                column: "PlaceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlaceSliderPhotos");
        }
    }
}
