using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rapido.Services.Wallets.Infrastructure.EF.Migrations
{
    /// <inheritdoc />
    public partial class RapidoWalletsOwnerVerificationRemoved : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VerifiedAt",
                table: "Owner");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "VerifiedAt",
                table: "Owner",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
