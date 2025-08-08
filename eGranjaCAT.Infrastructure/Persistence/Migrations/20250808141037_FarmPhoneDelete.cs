using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eGranjaCAT.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FarmPhoneDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Farms");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Farms",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
