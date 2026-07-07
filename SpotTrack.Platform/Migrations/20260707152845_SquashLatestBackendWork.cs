using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace SpotTrack.Platform.Migrations
{
    /// <inheritdoc />
    public partial class SquashLatestBackendWork : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "city",
                table: "gyms");

            migrationBuilder.DropColumn(
                name: "district",
                table: "gyms");

            migrationBuilder.DropColumn(
                name: "street",
                table: "gyms");

            migrationBuilder.DropTable(
                name: "anomalies");

            migrationBuilder.AddColumn<string>(
                name: "password_reset_code_hash",
                table: "users",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "password_reset_expires_at",
                table: "users",
                type: "datetime",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "created_at",
                table: "roi_projections",
                type: "datetime",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "updated_at",
                table: "roi_projections",
                type: "datetime",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "created_at",
                table: "maintenance_quotes",
                type: "datetime",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "updated_at",
                table: "maintenance_quotes",
                type: "datetime",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "maintenance_threshold",
                table: "equipment",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "created_at",
                table: "activity_reports",
                type: "datetime",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "updated_at",
                table: "activity_reports",
                type: "datetime",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "anomalies",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    admin_id = table.Column<int>(type: "int", nullable: false),
                    sensor_id = table.Column<int>(type: "int", nullable: false),
                    anomaly_type = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false),
                    detected_at = table.Column<DateTimeOffset>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_anomalies", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "businesses",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    admin_id = table.Column<int>(type: "int", nullable: false),
                    company_name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    ruc = table.Column<string>(type: "varchar(11)", maxLength: 11, nullable: false),
                    legal_structure = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    company_phone = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: false),
                    company_email = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_businesses", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "gym_authorized_dnis",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    gym_id = table.Column<int>(type: "int", nullable: false),
                    dni = table.Column<string>(type: "varchar(8)", maxLength: 8, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_gym_authorized_dnis", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "payments",
                columns: table => new
                {
                    payment_id = table.Column<Guid>(type: "char(36)", nullable: false),
                    user_id = table.Column<int>(type: "int", nullable: true),
                    pending_registration_id = table.Column<Guid>(type: "char(36)", nullable: true),
                    membership_id = table.Column<int>(type: "int", nullable: true),
                    membership_plan = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    amount = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    currency = table.Column<string>(type: "varchar(3)", maxLength: 3, nullable: false),
                    status = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    gateway_transaction_id = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true),
                    purpose = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_payments", x => x.payment_id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "pending_registrations",
                columns: table => new
                {
                    registration_id = table.Column<Guid>(type: "char(36)", nullable: false),
                    email = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    hashed_password = table.Column<string>(type: "longtext", nullable: false),
                    first_name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    last_name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    phone_number = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    dni = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    company_name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    ruc = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    legal_structure = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    company_phone = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    company_email = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    street_address = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    city = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    district = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    membership_tier = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetime", nullable: false),
                    expires_at = table.Column<DateTimeOffset>(type: "datetime", nullable: false),
                    status = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_pending_registrations", x => x.registration_id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "sensors",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    type = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    identifier = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    admin_id = table.Column<int>(type: "int", nullable: false),
                    equipment_id = table.Column<int>(type: "int", nullable: true),
                    registered_at = table.Column<DateTimeOffset>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_sensors", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "session_trackers",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    equipment_id = table.Column<int>(type: "int", nullable: false),
                    admin_id = table.Column<int>(type: "int", nullable: false),
                    started_at = table.Column<DateTimeOffset>(type: "datetime", nullable: false),
                    ended_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_session_trackers", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "technicians",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    specialization = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    phone_number = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    admin_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_technicians", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "sensor_captures",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    detected_at = table.Column<DateTimeOffset>(type: "datetime", nullable: false),
                    sensor_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_sensor_captures", x => x.id);
                    table.ForeignKey(
                        name: "f_k_sensor_captures_sensors_sensor_id",
                        column: x => x.sensor_id,
                        principalTable: "sensors",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "i_x_businesses_admin_id",
                table: "businesses",
                column: "admin_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_x_gym_authorized_dnis_gym_id_dni",
                table: "gym_authorized_dnis",
                columns: new[] { "gym_id", "dni" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_x_pending_registrations_email",
                table: "pending_registrations",
                column: "email");

            migrationBuilder.CreateIndex(
                name: "i_x_sensor_captures_sensor_id",
                table: "sensor_captures",
                column: "sensor_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "businesses");

            migrationBuilder.DropTable(
                name: "gym_authorized_dnis");

            migrationBuilder.DropTable(
                name: "payments");

            migrationBuilder.DropTable(
                name: "pending_registrations");

            migrationBuilder.DropTable(
                name: "sensor_captures");

            migrationBuilder.DropTable(
                name: "session_trackers");

            migrationBuilder.DropTable(
                name: "technicians");

            migrationBuilder.DropTable(
                name: "sensors");

            migrationBuilder.DropColumn(
                name: "password_reset_code_hash",
                table: "users");

            migrationBuilder.DropColumn(
                name: "password_reset_expires_at",
                table: "users");

            migrationBuilder.DropColumn(
                name: "created_at",
                table: "roi_projections");

            migrationBuilder.DropColumn(
                name: "updated_at",
                table: "roi_projections");

            migrationBuilder.DropColumn(
                name: "created_at",
                table: "maintenance_quotes");

            migrationBuilder.DropColumn(
                name: "updated_at",
                table: "maintenance_quotes");

            migrationBuilder.DropColumn(
                name: "maintenance_threshold",
                table: "equipment");

            migrationBuilder.DropColumn(
                name: "created_at",
                table: "activity_reports");

            migrationBuilder.DropColumn(
                name: "updated_at",
                table: "activity_reports");

            migrationBuilder.AddColumn<string>(
                name: "city",
                table: "gyms",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "district",
                table: "gyms",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "street",
                table: "gyms",
                type: "varchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.DropTable(
                name: "anomalies");

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
        }
    }
}
