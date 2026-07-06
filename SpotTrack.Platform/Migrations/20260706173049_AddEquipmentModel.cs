using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpotTrack.Platform.Migrations
{
    /// <inheritdoc />
    public partial class AddEquipmentModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "model",
                table: "equipment",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "model",
                table: "equipment");
        }
    }
}
