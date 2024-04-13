using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rapido.Services.Customers.Core.EF.Migrations
{
    /// <inheritdoc />
    public partial class Rapido_Customers_Customer_State_Before_Lockout : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StateBeforeLockout",
                table: "Customers",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StateBeforeLockout",
                table: "Customers");
        }
    }
}
