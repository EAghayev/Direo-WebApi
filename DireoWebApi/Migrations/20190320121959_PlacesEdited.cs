using Microsoft.EntityFrameworkCore.Migrations;

namespace DireoWebApi.Migrations
{
    public partial class PlacesEdited : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Ranking",
                table: "Places",
                nullable: true
                );

            migrationBuilder.AlterColumn<decimal>(
               name: "ReviewCount",
               table: "Places",
               nullable: true
               );

            migrationBuilder.AlterColumn<bool>(
             name: "Gender",
             table: "Places",
             nullable: true
             );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
               name: "Ranking",
               table: "Places",
               nullable: true
               );

            migrationBuilder.AlterColumn<decimal>(
             name: "ReviewCount",
             table: "Places",
             nullable: true
             );

            migrationBuilder.AlterColumn<bool>(
          name: "Gender",
          table: "Places",
          nullable: true
          );
        }
    }
}
