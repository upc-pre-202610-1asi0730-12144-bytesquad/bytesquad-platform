using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpotTrack.Platform.Migrations
{
    /// <inheritdoc />
    public partial class AddAnalyticsAuditColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
                name: "created_at",
                table: "activity_reports");

            migrationBuilder.DropColumn(
                name: "updated_at",
                table: "activity_reports");
        }
    }
}
