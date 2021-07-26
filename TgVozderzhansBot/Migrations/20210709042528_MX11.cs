using Microsoft.EntityFrameworkCore.Migrations;

namespace TgVozderzhansBot.Migrations
{
    public partial class MX11 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "OwnerId",
                table: "MailingItems",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MailingItems_OwnerId",
                table: "MailingItems",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_MailingItems_Users_OwnerId",
                table: "MailingItems",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MailingItems_Users_OwnerId",
                table: "MailingItems");

            migrationBuilder.DropIndex(
                name: "IX_MailingItems_OwnerId",
                table: "MailingItems");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "MailingItems");
        }
    }
}
