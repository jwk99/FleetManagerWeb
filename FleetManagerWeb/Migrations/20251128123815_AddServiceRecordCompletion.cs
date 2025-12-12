using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FleetManagerWeb.Migrations
{
    /// <inheritdoc />
    public partial class AddServiceRecordCompletion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CompletedDate",
                table: "ServiceRecords",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCompleted",
                table: "ServiceRecords",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompletedDate",
                table: "ServiceRecords");

            migrationBuilder.DropColumn(
                name: "IsCompleted",
                table: "ServiceRecords");
        }
    }
}
