using Microsoft.EntityFrameworkCore.Migrations;

namespace TgVozderzhansBot.Migrations
{
    public partial class MX8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "FriendId",
                table: "InputMessageToFriends",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_InputMessageToFriends_FriendId",
                table: "InputMessageToFriends",
                column: "FriendId");

            migrationBuilder.AddForeignKey(
                name: "FK_InputMessageToFriends_Users_FriendId",
                table: "InputMessageToFriends",
                column: "FriendId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InputMessageToFriends_Users_FriendId",
                table: "InputMessageToFriends");

            migrationBuilder.DropIndex(
                name: "IX_InputMessageToFriends_FriendId",
                table: "InputMessageToFriends");

            migrationBuilder.DropColumn(
                name: "FriendId",
                table: "InputMessageToFriends");
        }
    }
}
