using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JassTournamentManager.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "jtm");

            migrationBuilder.CreateTable(
                name: "users",
                schema: "jtm",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    email = table.Column<string>(type: "character varying(320)", maxLength: 320, nullable: true),
                    password_hash = table.Column<string>(type: "text", nullable: true),
                    first_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    last_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    is_sys_admin = table.Column<bool>(type: "boolean", nullable: false),
                    source_type = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    merge_target_user_id = table.Column<Guid>(type: "uuid", nullable: true),
                    merged_by = table.Column<Guid>(type: "uuid", nullable: true),
                    merged_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                    table.ForeignKey(
                        name: "FK_users_users_merge_target_user_id",
                        column: x => x.merge_target_user_id,
                        principalSchema: "jtm",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_users_users_merged_by",
                        column: x => x.merged_by,
                        principalSchema: "jtm",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "jass_tables",
                schema: "jtm",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    organizer_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    display_order = table.Column<int>(type: "integer", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_jass_tables", x => x.id);
                    table.ForeignKey(
                        name: "FK_jass_tables_users_organizer_id",
                        column: x => x.organizer_id,
                        principalSchema: "jtm",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tournament_templates",
                schema: "jtm",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    organizer_id = table.Column<Guid>(type: "uuid", nullable: false),
                    number_of_rounds = table.Column<int>(type: "integer", nullable: false),
                    games_per_round = table.Column<int>(type: "integer", nullable: false),
                    match_bonus_enabled = table.Column<bool>(type: "boolean", nullable: false),
                    is_fixed_teams = table.Column<bool>(type: "boolean", nullable: false),
                    score_visibility = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    location = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tournament_templates", x => x.id);
                    table.ForeignKey(
                        name: "FK_tournament_templates_users_organizer_id",
                        column: x => x.organizer_id,
                        principalSchema: "jtm",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tournaments",
                schema: "jtm",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    organizer_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    location = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    date = table.Column<DateOnly>(type: "date", nullable: false),
                    tournament_code = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    number_of_rounds = table.Column<int>(type: "integer", nullable: false),
                    games_per_round = table.Column<int>(type: "integer", nullable: false),
                    match_bonus_enabled = table.Column<bool>(type: "boolean", nullable: false),
                    is_fixed_teams = table.Column<bool>(type: "boolean", nullable: false),
                    score_visibility = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    status = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tournaments", x => x.id);
                    table.ForeignKey(
                        name: "FK_tournaments_users_organizer_id",
                        column: x => x.organizer_id,
                        principalSchema: "jtm",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "rounds",
                schema: "jtm",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    tournament_id = table.Column<Guid>(type: "uuid", nullable: false),
                    round_number = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rounds", x => x.id);
                    table.ForeignKey(
                        name: "FK_rounds_tournaments_tournament_id",
                        column: x => x.tournament_id,
                        principalSchema: "jtm",
                        principalTable: "tournaments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tournament_participants",
                schema: "jtm",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    tournament_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    registration_method = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    role = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    is_playing = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tournament_participants", x => x.id);
                    table.ForeignKey(
                        name: "FK_tournament_participants_tournaments_tournament_id",
                        column: x => x.tournament_id,
                        principalSchema: "jtm",
                        principalTable: "tournaments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tournament_participants_users_user_id",
                        column: x => x.user_id,
                        principalSchema: "jtm",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "pairings",
                schema: "jtm",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    round_id = table.Column<Guid>(type: "uuid", nullable: false),
                    jass_table_id = table.Column<Guid>(type: "uuid", nullable: false),
                    games_per_round = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pairings", x => x.id);
                    table.ForeignKey(
                        name: "FK_pairings_jass_tables_jass_table_id",
                        column: x => x.jass_table_id,
                        principalSchema: "jtm",
                        principalTable: "jass_tables",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_pairings_rounds_round_id",
                        column: x => x.round_id,
                        principalSchema: "jtm",
                        principalTable: "rounds",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "games",
                schema: "jtm",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    pairing_id = table.Column<Guid>(type: "uuid", nullable: false),
                    game_number = table.Column<int>(type: "integer", nullable: false),
                    match_bonus_enabled = table.Column<bool>(type: "boolean", nullable: false),
                    status = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    team_a_points = table.Column<int>(type: "integer", nullable: true),
                    team_b_points = table.Column<int>(type: "integer", nullable: true),
                    team_a_match_bonus_achieved = table.Column<bool>(type: "boolean", nullable: true),
                    team_b_match_bonus_achieved = table.Column<bool>(type: "boolean", nullable: true),
                    entered_by = table.Column<Guid>(type: "uuid", nullable: true),
                    entered_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_games", x => x.id);
                    table.ForeignKey(
                        name: "FK_games_pairings_pairing_id",
                        column: x => x.pairing_id,
                        principalSchema: "jtm",
                        principalTable: "pairings",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_games_tournament_participants_entered_by",
                        column: x => x.entered_by,
                        principalSchema: "jtm",
                        principalTable: "tournament_participants",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "pairing_participants",
                schema: "jtm",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    pairing_id = table.Column<Guid>(type: "uuid", nullable: false),
                    tournament_participant_id = table.Column<Guid>(type: "uuid", nullable: false),
                    team = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    entered_by = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pairing_participants", x => x.id);
                    table.ForeignKey(
                        name: "FK_pairing_participants_pairings_pairing_id",
                        column: x => x.pairing_id,
                        principalSchema: "jtm",
                        principalTable: "pairings",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_pairing_participants_tournament_participants_entered_by",
                        column: x => x.entered_by,
                        principalSchema: "jtm",
                        principalTable: "tournament_participants",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_pairing_participants_tournament_participants_tournament_par~",
                        column: x => x.tournament_participant_id,
                        principalSchema: "jtm",
                        principalTable: "tournament_participants",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "idx_game_pairing",
                schema: "jtm",
                table: "games",
                column: "pairing_id");

            migrationBuilder.CreateIndex(
                name: "IX_games_entered_by",
                schema: "jtm",
                table: "games",
                column: "entered_by");

            migrationBuilder.CreateIndex(
                name: "ux_games_pairing_game_number",
                schema: "jtm",
                table: "games",
                columns: new[] { "pairing_id", "game_number" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_table_active",
                schema: "jtm",
                table: "jass_tables",
                columns: new[] { "organizer_id", "is_active" });

            migrationBuilder.CreateIndex(
                name: "idx_table_organizer",
                schema: "jtm",
                table: "jass_tables",
                column: "organizer_id");

            migrationBuilder.CreateIndex(
                name: "idx_pairing_participant_pairing",
                schema: "jtm",
                table: "pairing_participants",
                column: "pairing_id");

            migrationBuilder.CreateIndex(
                name: "idx_pairing_participant_participant",
                schema: "jtm",
                table: "pairing_participants",
                column: "tournament_participant_id");

            migrationBuilder.CreateIndex(
                name: "IX_pairing_participants_entered_by",
                schema: "jtm",
                table: "pairing_participants",
                column: "entered_by");

            migrationBuilder.CreateIndex(
                name: "ux_pairing_participants_pairing_tournament_participant",
                schema: "jtm",
                table: "pairing_participants",
                columns: new[] { "pairing_id", "tournament_participant_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_pairing_round",
                schema: "jtm",
                table: "pairings",
                column: "round_id");

            migrationBuilder.CreateIndex(
                name: "IX_pairings_jass_table_id",
                schema: "jtm",
                table: "pairings",
                column: "jass_table_id");

            migrationBuilder.CreateIndex(
                name: "ux_pairings_round_jass_table",
                schema: "jtm",
                table: "pairings",
                columns: new[] { "round_id", "jass_table_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_round_tournament",
                schema: "jtm",
                table: "rounds",
                column: "tournament_id");

            migrationBuilder.CreateIndex(
                name: "ux_rounds_tournament_round_number",
                schema: "jtm",
                table: "rounds",
                columns: new[] { "tournament_id", "round_number" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_participant_tournament",
                schema: "jtm",
                table: "tournament_participants",
                column: "tournament_id");

            migrationBuilder.CreateIndex(
                name: "idx_participant_user",
                schema: "jtm",
                table: "tournament_participants",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ux_tournament_participants_user_tournament",
                schema: "jtm",
                table: "tournament_participants",
                columns: new[] { "user_id", "tournament_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_tournament_template_organizer",
                schema: "jtm",
                table: "tournament_templates",
                column: "organizer_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_tournament_organizer",
                schema: "jtm",
                table: "tournaments",
                column: "organizer_id");

            migrationBuilder.CreateIndex(
                name: "idx_tournament_status",
                schema: "jtm",
                table: "tournaments",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "ux_tournaments_tournament_code",
                schema: "jtm",
                table: "tournaments",
                column: "tournament_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_users_merge_target_user_id",
                schema: "jtm",
                table: "users",
                column: "merge_target_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_users_merged_by",
                schema: "jtm",
                table: "users",
                column: "merged_by");

            migrationBuilder.CreateIndex(
                name: "ux_users_email",
                schema: "jtm",
                table: "users",
                column: "email",
                unique: true)
                .Annotation("Npgsql:NullsDistinct", true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "games",
                schema: "jtm");

            migrationBuilder.DropTable(
                name: "pairing_participants",
                schema: "jtm");

            migrationBuilder.DropTable(
                name: "tournament_templates",
                schema: "jtm");

            migrationBuilder.DropTable(
                name: "pairings",
                schema: "jtm");

            migrationBuilder.DropTable(
                name: "tournament_participants",
                schema: "jtm");

            migrationBuilder.DropTable(
                name: "jass_tables",
                schema: "jtm");

            migrationBuilder.DropTable(
                name: "rounds",
                schema: "jtm");

            migrationBuilder.DropTable(
                name: "tournaments",
                schema: "jtm");

            migrationBuilder.DropTable(
                name: "users",
                schema: "jtm");
        }
    }
}
