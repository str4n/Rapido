using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rapido.Services.Wallets.Infrastructure.EF.Migrations
{
    /// <inheritdoc />
    public partial class Rapido_Wallets_Wallet_Logic_Refactor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amount",
                table: "Balance");

            migrationBuilder.CreateTable(
                name: "SubTransfer",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BalanceId = table.Column<Guid>(type: "uuid", nullable: true),
                    Metadata = table.Column<string>(type: "text", nullable: false),
                    Currency = table.Column<string>(type: "text", nullable: false),
                    Amount = table.Column<double>(type: "double precision", nullable: false),
                    ExchangeRate = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    TransferId = table.Column<Guid>(type: "uuid", nullable: true),
                    Type = table.Column<string>(type: "character varying(21)", maxLength: 21, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubTransfer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubTransfer_Balance_BalanceId",
                        column: x => x.BalanceId,
                        principalTable: "Balance",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SubTransfer_Transfer_TransferId",
                        column: x => x.TransferId,
                        principalTable: "Transfer",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_SubTransfer_BalanceId",
                table: "SubTransfer",
                column: "BalanceId");

            migrationBuilder.CreateIndex(
                name: "IX_SubTransfer_TransferId",
                table: "SubTransfer",
                column: "TransferId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SubTransfer");

            migrationBuilder.AddColumn<double>(
                name: "Amount",
                table: "Balance",
                type: "double precision",
                nullable: true);
        }
    }
}
