using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addeventvoucher : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_voucher_event_event_id",
                table: "voucher");

            migrationBuilder.RenameColumn(
                name: "event_id",
                table: "voucher",
                newName: "brand_id");

            migrationBuilder.RenameIndex(
                name: "IX_voucher_event_id",
                table: "voucher",
                newName: "IX_voucher_brand_id");

            migrationBuilder.CreateTable(
                name: "brand_voucher",
                columns: table => new
                {
                    EventId = table.Column<int>(type: "integer", nullable: false),
                    VoucherId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_brand_voucher", x => new { x.EventId, x.VoucherId });
                    table.ForeignKey(
                        name: "FK_brand_voucher_event_EventId",
                        column: x => x.EventId,
                        principalTable: "event",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_brand_voucher_voucher_VoucherId",
                        column: x => x.VoucherId,
                        principalTable: "voucher",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_brand_voucher_VoucherId",
                table: "brand_voucher",
                column: "VoucherId");

            migrationBuilder.AddForeignKey(
                name: "FK_voucher_brand_brand_id",
                table: "voucher",
                column: "brand_id",
                principalTable: "brand",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_voucher_brand_brand_id",
                table: "voucher");

            migrationBuilder.DropTable(
                name: "brand_voucher");

            migrationBuilder.RenameColumn(
                name: "brand_id",
                table: "voucher",
                newName: "event_id");

            migrationBuilder.RenameIndex(
                name: "IX_voucher_brand_id",
                table: "voucher",
                newName: "IX_voucher_event_id");

            migrationBuilder.AddForeignKey(
                name: "FK_voucher_event_event_id",
                table: "voucher",
                column: "event_id",
                principalTable: "event",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
