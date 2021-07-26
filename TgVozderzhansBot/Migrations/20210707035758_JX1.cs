using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace TgVozderzhansBot.Migrations
{
    public partial class JX1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AbsGuardItems",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<long>(type: "bigint", nullable: true),
                    AbsItemId = table.Column<long>(type: "bigint", nullable: true),
                    LastNotify = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    IsConfirm = table.Column<bool>(type: "boolean", nullable: false),
                    ConfirmMs = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbsGuardItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbsGuardItems_AbsItems_AbsItemId",
                        column: x => x.AbsItemId,
                        principalTable: "AbsItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AbsGuardItems_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AbsGuardItems_AbsItemId",
                table: "AbsGuardItems",
                column: "AbsItemId");

            migrationBuilder.CreateIndex(
                name: "IX_AbsGuardItems_UserId",
                table: "AbsGuardItems",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AbsGuardItems");
        }
    }
}
