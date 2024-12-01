using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rapido.Saga.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Rapido_Saga_Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccountSetUpSagaData",
                columns: table => new
                {
                    CorrelationId = table.Column<Guid>(type: "uuid", nullable: false),
                    CurrentState = table.Column<string>(type: "text", nullable: true),
                    AccountId = table.Column<Guid>(type: "uuid", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: true),
                    AccountType = table.Column<string>(type: "text", nullable: true),
                    UserActivated = table.Column<bool>(type: "boolean", nullable: false),
                    CustomerCreated = table.Column<bool>(type: "boolean", nullable: false),
                    CustomerCompleted = table.Column<bool>(type: "boolean", nullable: false),
                    OwnerCreated = table.Column<bool>(type: "boolean", nullable: false),
                    WalletCreated = table.Column<bool>(type: "boolean", nullable: false),
                    AccountSetUpCompleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountSetUpSagaData", x => x.CorrelationId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountSetUpSagaData");
        }
    }
}
