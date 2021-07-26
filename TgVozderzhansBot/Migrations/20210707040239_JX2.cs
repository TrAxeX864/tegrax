using Microsoft.EntityFrameworkCore.Migrations;

namespace TgVozderzhansBot.Migrations
{
    public partial class JX2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "UserId1",
                table: "AbsItems",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UserId2",
                table: "AbsItems",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AbsItems_UserId1",
                table: "AbsItems",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_AbsItems_UserId2",
                table: "AbsItems",
                column: "UserId2");

            migrationBuilder.AddForeignKey(
                name: "FK_AbsItems_Users_UserId1",
                table: "AbsItems",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AbsItems_Users_UserId2",
                table: "AbsItems",
                column: "UserId2",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AbsItems_Users_UserId1",
                table: "AbsItems");

            migrationBuilder.DropForeignKey(
                name: "FK_AbsItems_Users_UserId2",
                table: "AbsItems");

            migrationBuilder.DropIndex(
                name: "IX_AbsItems_UserId1",
                table: "AbsItems");

            migrationBuilder.DropIndex(
                name: "IX_AbsItems_UserId2",
                table: "AbsItems");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "AbsItems");

            migrationBuilder.DropColumn(
                name: "UserId2",
                table: "AbsItems");
        }
    }
}
