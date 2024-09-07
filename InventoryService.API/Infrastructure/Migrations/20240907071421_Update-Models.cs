using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventoryService.API.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ItemId",
                table: "ItemPieces",
                newName: "ItemPieceId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "item",
                newName: "id");

            migrationBuilder.AddColumn<int>(
                name: "EventId",
                table: "ItemPieces",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "GameItemId",
                table: "ItemPieces",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EventId",
                table: "ItemPieces");

            migrationBuilder.DropColumn(
                name: "GameItemId",
                table: "ItemPieces");

            migrationBuilder.RenameColumn(
                name: "ItemPieceId",
                table: "ItemPieces",
                newName: "ItemId");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "item",
                newName: "Id");
        }
    }
}
