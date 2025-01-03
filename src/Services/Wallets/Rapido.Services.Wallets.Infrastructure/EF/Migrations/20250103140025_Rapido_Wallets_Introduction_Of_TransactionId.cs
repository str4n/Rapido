using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rapido.Services.Wallets.Infrastructure.EF.Migrations
{
    /// <inheritdoc />
    public partial class Rapido_Wallets_Introduction_Of_TransactionId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SubTransfer");

            migrationBuilder.AddColumn<string>(
                name: "TransactionId",
                table: "Transfer",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "InternalTransfer",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TransactionId = table.Column<string>(type: "text", nullable: true),
                    BalanceId = table.Column<Guid>(type: "uuid", nullable: true),
                    Metadata = table.Column<string>(type: "text", nullable: true),
                    Currency = table.Column<string>(type: "text", nullable: false),
                    Amount = table.Column<double>(type: "double precision", nullable: false),
                    ExchangeRate = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    TransferId = table.Column<Guid>(type: "uuid", nullable: true),
                    Type = table.Column<string>(type: "character varying(34)", maxLength: 34, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InternalTransfer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InternalTransfer_Balance_BalanceId",
                        column: x => x.BalanceId,
                        principalTable: "Balance",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_InternalTransfer_Transfer_TransferId",
                        column: x => x.TransferId,
                        principalTable: "Transfer",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_InternalTransfer_BalanceId",
                table: "InternalTransfer",
                column: "BalanceId");

            migrationBuilder.CreateIndex(
                name: "IX_InternalTransfer_TransferId",
                table: "InternalTransfer",
                column: "TransferId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InternalTransfer");

            migrationBuilder.DropColumn(
                name: "TransactionId",
                table: "Transfer");

            migrationBuilder.CreateTable(
                name: "SubTransfer",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Amount = table.Column<double>(type: "double precision", nullable: false),
                    BalanceId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Currency = table.Column<string>(type: "text", nullable: false),
                    ExchangeRate = table.Column<string>(type: "text", nullable: true),
                    Metadata = table.Column<string>(type: "text", nullable: true),
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
    }
}
