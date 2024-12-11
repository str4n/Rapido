using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rapido.Saga.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Rapido_Saga_RecipientCreatedState : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "RecipientCreated",
                table: "AccountSetUpSagaData",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RecipientCreated",
                table: "AccountSetUpSagaData");
        }
    }
}
