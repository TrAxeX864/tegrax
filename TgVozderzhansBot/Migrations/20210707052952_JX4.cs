using Microsoft.EntityFrameworkCore.Migrations;

namespace TgVozderzhansBot.Migrations
{
    public partial class JX4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Flag",
                table: "AbsGuardItems",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Flag",
                table: "AbsGuardItems");
        }
    }
}
