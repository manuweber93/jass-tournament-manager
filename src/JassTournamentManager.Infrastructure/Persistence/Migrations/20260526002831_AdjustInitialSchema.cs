using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JassTournamentManager.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AdjustInitialSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_pairing_participants_tournament_participants_entered_by",
                schema: "jtm",
                table: "pairing_participants");

            migrationBuilder.DropForeignKey(
                name: "FK_users_users_merge_target_user_id",
                schema: "jtm",
                table: "users");

            migrationBuilder.AddForeignKey(
                name: "FK_pairing_participants_tournament_participants_entered_by",
                schema: "jtm",
                table: "pairing_participants",
                column: "entered_by",
                principalSchema: "jtm",
                principalTable: "tournament_participants",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_users_users_merge_target_user_id",
                schema: "jtm",
                table: "users",
                column: "merge_target_user_id",
                principalSchema: "jtm",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_pairing_participants_tournament_participants_entered_by",
                schema: "jtm",
                table: "pairing_participants");

            migrationBuilder.DropForeignKey(
                name: "FK_users_users_merge_target_user_id",
                schema: "jtm",
                table: "users");

            migrationBuilder.AddForeignKey(
                name: "FK_pairing_participants_tournament_participants_entered_by",
                schema: "jtm",
                table: "pairing_participants",
                column: "entered_by",
                principalSchema: "jtm",
                principalTable: "tournament_participants",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_users_users_merge_target_user_id",
                schema: "jtm",
                table: "users",
                column: "merge_target_user_id",
                principalSchema: "jtm",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
