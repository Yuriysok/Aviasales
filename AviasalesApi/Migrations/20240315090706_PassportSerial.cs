using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AviasalesApi.Migrations
{
    /// <inheritdoc />
    public partial class PassportSerial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PassportSerialNumber",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Users_PassportSerialNumber",
                table: "Users",
                column: "PassportSerialNumber",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_PassportSerialNumber",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PassportSerialNumber",
                table: "Users");
        }
    }
}
