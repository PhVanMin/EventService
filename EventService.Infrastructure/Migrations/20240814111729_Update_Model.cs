using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Update_Model : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VerifyKey",
                table: "voucher");

            migrationBuilder.AddColumn<int>(
                name: "EventId",
                table: "redeem_voucher",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RedeemVoucherCount",
                table: "event",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_redeem_voucher_EventId",
                table: "redeem_voucher",
                column: "EventId");

            migrationBuilder.AddForeignKey(
                name: "FK_redeem_voucher_event_EventId",
                table: "redeem_voucher",
                column: "EventId",
                principalTable: "event",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_redeem_voucher_event_EventId",
                table: "redeem_voucher");

            migrationBuilder.DropIndex(
                name: "IX_redeem_voucher_EventId",
                table: "redeem_voucher");

            migrationBuilder.DropColumn(
                name: "EventId",
                table: "redeem_voucher");

            migrationBuilder.DropColumn(
                name: "RedeemVoucherCount",
                table: "event");

            migrationBuilder.AddColumn<string>(
                name: "VerifyKey",
                table: "voucher",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
