using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rapido.Services.Wallets.Infrastructure.EF.Migrations
{
    /// <inheritdoc />
    public partial class Rapido_Wallets_TransfersColumnRename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InternalTransfer_Transfer_TransferId",
                table: "InternalTransfer");

            migrationBuilder.DropForeignKey(
                name: "FK_Transfer_Wallets_WalletId",
                table: "Transfer");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Transfer",
                table: "Transfer");

            migrationBuilder.RenameTable(
                name: "Transfer",
                newName: "Transfers");

            migrationBuilder.RenameIndex(
                name: "IX_Transfer_WalletId",
                table: "Transfers",
                newName: "IX_Transfers_WalletId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Transfers",
                table: "Transfers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InternalTransfer_Transfers_TransferId",
                table: "InternalTransfer",
                column: "TransferId",
                principalTable: "Transfers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_Wallets_WalletId",
                table: "Transfers",
                column: "WalletId",
                principalTable: "Wallets",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InternalTransfer_Transfers_TransferId",
                table: "InternalTransfer");

            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_Wallets_WalletId",
                table: "Transfers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Transfers",
                table: "Transfers");

            migrationBuilder.RenameTable(
                name: "Transfers",
                newName: "Transfer");

            migrationBuilder.RenameIndex(
                name: "IX_Transfers_WalletId",
                table: "Transfer",
                newName: "IX_Transfer_WalletId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Transfer",
                table: "Transfer",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InternalTransfer_Transfer_TransferId",
                table: "InternalTransfer",
                column: "TransferId",
                principalTable: "Transfer",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Transfer_Wallets_WalletId",
                table: "Transfer",
                column: "WalletId",
                principalTable: "Wallets",
                principalColumn: "Id");
        }
    }
}
