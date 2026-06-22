using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace SpotTrack.Platform.Migrations
{
    /// <inheritdoc />
    public partial class AddAnalytics : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "activity_reports",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    activity_report_id = table.Column<long>(type: "bigint", nullable: false),
                    total_usage_time = table.Column<long>(type: "bigint", nullable: false),
                    downtime_cost = table.Column<long>(type: "bigint", nullable: false),
                    percentage_comparison = table.Column<double>(type: "double", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_activity_reports", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "maintenance_quotes",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    maintenance_quote_id = table.Column<long>(type: "bigint", nullable: false),
                    corrective_actions_cost = table.Column<double>(type: "double", nullable: false),
                    spare_parts_cost = table.Column<double>(type: "double", nullable: false),
                    preventive_cost = table.Column<double>(type: "double", nullable: false),
                    total_maintenance_cost = table.Column<double>(type: "double", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_maintenance_quotes", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "roi_projections",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    roi_projection_id = table.Column<long>(type: "bigint", nullable: false),
                    projected_downtime_cost = table.Column<double>(type: "double", nullable: false),
                    projected_earnings = table.Column<double>(type: "double", nullable: false),
                    roi_index = table.Column<double>(type: "double", nullable: false),
                    demand_status = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_roi_projections", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "activity_reports");

            migrationBuilder.DropTable(
                name: "maintenance_quotes");

            migrationBuilder.DropTable(
                name: "roi_projections");
        }
    }
}
