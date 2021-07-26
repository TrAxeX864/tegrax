using Microsoft.EntityFrameworkCore.Migrations;

namespace TgVozderzhansBot.Migrations
{
    public partial class MX12 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "InviterChatId",
                table: "Users",
                type: "bigint",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InviterChatId",
                table: "Users");
        }
    }
}
