using Microsoft.EntityFrameworkCore.Migrations;

namespace TgVozderzhansBot.Migrations
{
    public partial class MX13 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsFriendGotPremium",
                table: "Users",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFriendGotPremium",
                table: "Users");
        }
    }
}
