using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpotTrack.Platform.Migrations
{
    /// <inheritdoc />
    public partial class AddPendingRegistration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateIndex(
                name: "i_x_pending_registrations_email",
                table: "pending_registrations",
                column: "email");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "pending_registrations");
        }
    }
}
