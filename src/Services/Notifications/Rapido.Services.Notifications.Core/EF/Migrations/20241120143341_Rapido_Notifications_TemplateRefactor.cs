using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rapido.Services.Notifications.Core.EF.Migrations
{
    /// <inheritdoc />
    public partial class Rapido_Notifications_TemplateRefactor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Templates",
                newName: "TemplatePath");

            migrationBuilder.RenameColumn(
                name: "Body",
                table: "Templates",
                newName: "Subject");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TemplatePath",
                table: "Templates",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "Subject",
                table: "Templates",
                newName: "Body");
        }
    }
}
