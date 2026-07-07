using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace SpotTrack.Platform.Migrations
{
    /// <inheritdoc />
    public partial class AddRoutineSessionBlockCompletions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "routine_session_block_completions",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    exercise_block_id = table.Column<int>(type: "int", nullable: false),
                    routine_session_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_routine_session_block_completions", x => x.id);
                    table.ForeignKey(
                        name: "f_k_session_block_completions_routine_sessions",
                        column: x => x.routine_session_id,
                        principalTable: "routine_sessions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "i_x_routine_session_block_completions_routine_session_id",
                table: "routine_session_block_completions",
                column: "routine_session_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "routine_session_block_completions");
        }
    }
}
