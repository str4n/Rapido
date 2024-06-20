using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rapido.Services.Wallets.Infrastructure.EF.Migrations
{
    /// <inheritdoc />
    public partial class RapidoWalletsMultiCurrencyWallet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Wallets_OwnerId_Currency",
                table: "Wallets");

            migrationBuilder.DropColumn(
                name: "Currency",
                table: "Wallets");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Transfer");

            migrationBuilder.CreateTable(
                name: "Balance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    WalletId = table.Column<Guid>(type: "uuid", nullable: true),
                    Amount = table.Column<double>(type: "double precision", nullable: true),
                    Currency = table.Column<string>(type: "text", nullable: true),
                    IsPrimary = table.Column<string>(type: "character varying(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Balance", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Balance_Wallets_WalletId",
                        column: x => x.WalletId,
                        principalTable: "Wallets",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Wallets_OwnerId",
                table: "Wallets",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Balance_WalletId",
                table: "Balance",
                column: "WalletId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Balance");

            migrationBuilder.DropIndex(
                name: "IX_Wallets_OwnerId",
                table: "Wallets");

            migrationBuilder.AddColumn<string>(
                name: "Currency",
                table: "Wallets",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Transfer",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Wallets_OwnerId_Currency",
                table: "Wallets",
                columns: new[] { "OwnerId", "Currency" },
                unique: true);
        }
    }
}
