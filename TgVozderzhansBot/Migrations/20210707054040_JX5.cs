using Microsoft.EntityFrameworkCore.Migrations;

namespace TgVozderzhansBot.Migrations
{
    public partial class JX5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Flag2",
                table: "AbsGuardItems",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Flag2",
                table: "AbsGuardItems");
        }
    }
}
