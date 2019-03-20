using Microsoft.EntityFrameworkCore.Migrations;

namespace DireoWebApi.Migrations
{
    public partial class TagsAndPlacesTagsCreated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 36, nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlacesTags",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 36, nullable: false),
                    PlaceId = table.Column<string>(maxLength: 36, nullable: true),
                    TagId = table.Column<string>(maxLength: 36, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlacesTags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlacesTags_Places_PlaceId",
                        column: x => x.PlaceId,
                        principalTable: "Places",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlacesTags_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlacesTags_PlaceId",
                table: "PlacesTags",
                column: "PlaceId");

            migrationBuilder.CreateIndex(
                name: "IX_PlacesTags_TagId",
                table: "PlacesTags",
                column: "TagId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlacesTags");

            migrationBuilder.DropTable(
                name: "Tags");
        }
    }
}
