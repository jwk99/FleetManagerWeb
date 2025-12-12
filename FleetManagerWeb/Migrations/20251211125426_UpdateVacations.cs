using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FleetManagerWeb.Migrations
{
    /// <inheritdoc />
    public partial class UpdateVacations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vacation_Drivers_DriverId",
                table: "Vacation");

            migrationBuilder.DropForeignKey(
                name: "FK_Vacation_Drivers_SubstituteDriverId",
                table: "Vacation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Vacation",
                table: "Vacation");

            migrationBuilder.RenameTable(
                name: "Vacation",
                newName: "Vacations");

            migrationBuilder.RenameIndex(
                name: "IX_Vacation_SubstituteDriverId",
                table: "Vacations",
                newName: "IX_Vacations_SubstituteDriverId");

            migrationBuilder.RenameIndex(
                name: "IX_Vacation_DriverId",
                table: "Vacations",
                newName: "IX_Vacations_DriverId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Vacations",
                table: "Vacations",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Vacations_Drivers_DriverId",
                table: "Vacations",
                column: "DriverId",
                principalTable: "Drivers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Vacations_Drivers_SubstituteDriverId",
                table: "Vacations",
                column: "SubstituteDriverId",
                principalTable: "Drivers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vacations_Drivers_DriverId",
                table: "Vacations");

            migrationBuilder.DropForeignKey(
                name: "FK_Vacations_Drivers_SubstituteDriverId",
                table: "Vacations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Vacations",
                table: "Vacations");

            migrationBuilder.RenameTable(
                name: "Vacations",
                newName: "Vacation");

            migrationBuilder.RenameIndex(
                name: "IX_Vacations_SubstituteDriverId",
                table: "Vacation",
                newName: "IX_Vacation_SubstituteDriverId");

            migrationBuilder.RenameIndex(
                name: "IX_Vacations_DriverId",
                table: "Vacation",
                newName: "IX_Vacation_DriverId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Vacation",
                table: "Vacation",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Vacation_Drivers_DriverId",
                table: "Vacation",
                column: "DriverId",
                principalTable: "Drivers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Vacation_Drivers_SubstituteDriverId",
                table: "Vacation",
                column: "SubstituteDriverId",
                principalTable: "Drivers",
                principalColumn: "Id");
        }
    }
}
