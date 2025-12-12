using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FleetManagerWeb.Migrations
{
    /// <inheritdoc />
    public partial class FixVehicleModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "year",
                table: "Vehicles",
                newName: "Year");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Year",
                table: "Vehicles",
                newName: "year");
        }
    }
}
