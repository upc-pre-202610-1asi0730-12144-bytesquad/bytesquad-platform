using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace SpotTrack.Platform.Migrations
{
    /// <inheritdoc />
    public partial class AddSensors : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "maximum_occupancy",
                table: "zones",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "timer_expiry",
                table: "reservations",
                type: "datetime",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "reps",
                table: "exercise_blocks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "sets",
                table: "exercise_blocks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "model",
                table: "equipment",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "created_at",
                table: "client_gym_associations",
                type: "datetime",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "updated_at",
                table: "client_gym_associations",
                type: "datetime",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "anomalies",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    reservation_id = table.Column<int>(type: "int", nullable: false),
                    equipment_id = table.Column<int>(type: "int", nullable: false),
                    zone_id = table.Column<int>(type: "int", nullable: false),
                    anomaly_description = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false),
                    emission_date = table.Column<DateTimeOffset>(type: "datetime", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_anomalies", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "sensors",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    equipment_id = table.Column<int>(type: "int", nullable: false),
                    mac_address = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    location = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    status = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    battery_level = table.Column<int>(type: "int", nullable: false),
                    signal_strength = table.Column<int>(type: "int", nullable: false),
                    firmware_version = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    last_heartbeat = table.Column<DateTimeOffset>(type: "datetime", nullable: false),
                    last_status_change_at = table.Column<DateTimeOffset>(type: "datetime", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_sensors", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "session_exercise_completions",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    exercise_block_id = table.Column<int>(type: "int", nullable: false),
                    routine_session_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_session_exercise_completions", x => x.id);
                    table.ForeignKey(
                        name: "fk_session_completions_routine_session",
                        column: x => x.routine_session_id,
                        principalTable: "routine_sessions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "i_x_session_exercise_completions_routine_session_id",
                table: "session_exercise_completions",
                column: "routine_session_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "anomalies");

            migrationBuilder.DropTable(
                name: "sensors");

            migrationBuilder.DropTable(
                name: "session_exercise_completions");

            migrationBuilder.DropColumn(
                name: "maximum_occupancy",
                table: "zones");

            migrationBuilder.DropColumn(
                name: "timer_expiry",
                table: "reservations");

            migrationBuilder.DropColumn(
                name: "reps",
                table: "exercise_blocks");

            migrationBuilder.DropColumn(
                name: "sets",
                table: "exercise_blocks");

            migrationBuilder.DropColumn(
                name: "model",
                table: "equipment");

            migrationBuilder.DropColumn(
                name: "created_at",
                table: "client_gym_associations");

            migrationBuilder.DropColumn(
                name: "updated_at",
                table: "client_gym_associations");
        }
    }
}
