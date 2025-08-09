using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eGranjaCAT.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class NewEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Updated",
                table: "Lots",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Updated",
                table: "Farms",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "UserGuid",
                table: "Farms",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Updated",
                table: "Lots");

            migrationBuilder.DropColumn(
                name: "Updated",
                table: "Farms");

            migrationBuilder.DropColumn(
                name: "UserGuid",
                table: "Farms");
        }
    }
}
