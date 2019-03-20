using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DireoWebApi.Migrations
{
    public partial class UsersCreated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 36, nullable: false),
                    FullName = table.Column<string>(maxLength: 120, nullable: false),
                    Password = table.Column<string>(maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    Profile = table.Column<string>(maxLength: 100, nullable: true),
                    ProfileFileName = table.Column<string>(maxLength: 100, nullable: true),
                    Email = table.Column<string>(maxLength: 100, nullable: true),
                    Website = table.Column<string>(maxLength: 250, nullable: true),
                    Phone = table.Column<string>(maxLength: 50, nullable: true),
                    Address = table.Column<string>(maxLength: 150, nullable: true),
                    Gender = table.Column<byte>(type: "tinyint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
