using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace SpotTrack.Platform.Migrations
{
    /// <inheritdoc />
    public partial class AddRoutineSelfServiceAndCompletions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                name: "session_exercise_completions");

            migrationBuilder.DropColumn(
                name: "reps",
                table: "exercise_blocks");

            migrationBuilder.DropColumn(
                name: "sets",
                table: "exercise_blocks");
        }
    }
}
