using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventoryService.API.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Create1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("user_pkey", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "item",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    gameItem_id = table.Column<Guid>(type: "uuid", nullable: false),
                    event_id = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("item_pkey", x => x.id);
                    table.ForeignKey(
                        name: "FK_item_user_UserId",
                        column: x => x.UserId,
                        principalTable: "user",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "item_piece",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    itemPiece_id = table.Column<Guid>(type: "uuid", nullable: false),
                    item_id = table.Column<Guid>(type: "uuid", nullable: false),
                    event_id = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("piece_pkey", x => x.id);
                    table.ForeignKey(
                        name: "FK_item_piece_user_UserId",
                        column: x => x.UserId,
                        principalTable: "user",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "voucher",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    event_id = table.Column<int>(type: "integer", nullable: false),
                    voucher_id = table.Column<int>(type: "integer", nullable: false),
                    code = table.Column<string>(type: "text", nullable: false),
                    expire_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("voucher_pkey", x => x.Id);
                    table.ForeignKey(
                        name: "FK_voucher_user_UserId",
                        column: x => x.UserId,
                        principalTable: "user",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_item_UserId",
                table: "item",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_item_piece_UserId",
                table: "item_piece",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_voucher_UserId",
                table: "voucher",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "item");

            migrationBuilder.DropTable(
                name: "item_piece");

            migrationBuilder.DropTable(
                name: "voucher");

            migrationBuilder.DropTable(
                name: "user");
        }
    }
}
