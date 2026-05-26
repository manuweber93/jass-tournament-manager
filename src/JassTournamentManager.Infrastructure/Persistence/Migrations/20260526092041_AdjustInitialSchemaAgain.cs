using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JassTournamentManager.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AdjustInitialSchemaAgain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_games_tournament_participants_entered_by",
                schema: "jtm",
                table: "games");

            migrationBuilder.DropForeignKey(
                name: "FK_pairing_participants_tournament_participants_entered_by",
                schema: "jtm",
                table: "pairing_participants");

            migrationBuilder.RenameColumn(
                name: "entered_by",
                schema: "jtm",
                table: "pairing_participants",
                newName: "entered_by_user_id");

            migrationBuilder.RenameIndex(
                name: "IX_pairing_participants_entered_by",
                schema: "jtm",
                table: "pairing_participants",
                newName: "IX_pairing_participants_entered_by_user_id");

            migrationBuilder.RenameColumn(
                name: "entered_by",
                schema: "jtm",
                table: "games",
                newName: "entered_by_user_id");

            migrationBuilder.RenameIndex(
                name: "IX_games_entered_by",
                schema: "jtm",
                table: "games",
                newName: "IX_games_entered_by_user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_games_users_entered_by_user_id",
                schema: "jtm",
                table: "games",
                column: "entered_by_user_id",
                principalSchema: "jtm",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_pairing_participants_users_entered_by_user_id",
                schema: "jtm",
                table: "pairing_participants",
                column: "entered_by_user_id",
                principalSchema: "jtm",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_games_users_entered_by_user_id",
                schema: "jtm",
                table: "games");

            migrationBuilder.DropForeignKey(
                name: "FK_pairing_participants_users_entered_by_user_id",
                schema: "jtm",
                table: "pairing_participants");

            migrationBuilder.RenameColumn(
                name: "entered_by_user_id",
                schema: "jtm",
                table: "pairing_participants",
                newName: "entered_by");

            migrationBuilder.RenameIndex(
                name: "IX_pairing_participants_entered_by_user_id",
                schema: "jtm",
                table: "pairing_participants",
                newName: "IX_pairing_participants_entered_by");

            migrationBuilder.RenameColumn(
                name: "entered_by_user_id",
                schema: "jtm",
                table: "games",
                newName: "entered_by");

            migrationBuilder.RenameIndex(
                name: "IX_games_entered_by_user_id",
                schema: "jtm",
                table: "games",
                newName: "IX_games_entered_by");

            migrationBuilder.AddForeignKey(
                name: "FK_games_tournament_participants_entered_by",
                schema: "jtm",
                table: "games",
                column: "entered_by",
                principalSchema: "jtm",
                principalTable: "tournament_participants",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_pairing_participants_tournament_participants_entered_by",
                schema: "jtm",
                table: "pairing_participants",
                column: "entered_by",
                principalSchema: "jtm",
                principalTable: "tournament_participants",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
