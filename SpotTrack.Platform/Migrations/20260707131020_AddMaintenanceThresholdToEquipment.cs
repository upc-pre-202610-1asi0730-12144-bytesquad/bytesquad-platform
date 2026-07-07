using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpotTrack.Platform.Migrations
{
    /// <inheritdoc />
    public partial class AddMaintenanceThresholdToEquipment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "maintenance_threshold",
                table: "equipment",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "maintenance_threshold",
                table: "equipment");
        }
    }
}
