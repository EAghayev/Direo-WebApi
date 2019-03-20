using Microsoft.EntityFrameworkCore.Migrations;

namespace DireoWebApi.Migrations
{
    public partial class SocialsCreated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Places",
                maxLength: 36,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Socials",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 36, nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    Link = table.Column<string>(maxLength: 250, nullable: false),
                    Type = table.Column<int>(nullable: true),
                    PlaceId = table.Column<string>(maxLength: 36, nullable: true),
                    UserId = table.Column<string>(maxLength: 36, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Socials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Socials_Places_PlaceId",
                        column: x => x.PlaceId,
                        principalTable: "Places",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Socials_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Places_UserId",
                table: "Places",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Socials_PlaceId",
                table: "Socials",
                column: "PlaceId");

            migrationBuilder.CreateIndex(
                name: "IX_Socials_UserId",
                table: "Socials",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Places_Users_UserId",
                table: "Places",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Places_Users_UserId",
                table: "Places");

            migrationBuilder.DropTable(
                name: "Socials");

            migrationBuilder.DropIndex(
                name: "IX_Places_UserId",
                table: "Places");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Places");
        }
    }
}
