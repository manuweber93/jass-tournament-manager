using FluentAssertions;
using JassTournamentManager.Domain.Entities;
using JassTournamentManager.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace JassTournamentManager.Infrastructure.Tests.Persistence
{
    [Collection(PostgreSqlCollection.Name)]
    public class DeleteBehaviorTests
    {
        private readonly PostgreSqlFixture _fixture;

        public DeleteBehaviorTests(PostgreSqlFixture fixture)
        {
            _fixture = fixture;
        }

        [DockerAvailableFact]
        public async Task Deleting_User_WithTournamentAsOrganizer_ThrowsDbUpdateException()
        {
            await _fixture.ResetDatabaseAsync();
            var organizer = PersistenceTestData.CreateUser(firstName: "Tournament", lastName: "Organizer");
            var tournament = PersistenceTestData.CreateTournament(organizer.Id, "DELETE-ORGANIZER");

            await using (var dbContext = _fixture.CreateDbContext())
            {
                dbContext.Users.Add(organizer);
                dbContext.Tournaments.Add(tournament);
                await dbContext.SaveChangesAsync();
            }

            await using var deleteContext = _fixture.CreateDbContext();
            deleteContext.Users.Remove(await deleteContext.Users.SingleAsync());

            var act = () => deleteContext.SaveChangesAsync();

            await act.Should().ThrowAsync<DbUpdateException>();
        }

        [DockerAvailableFact]
        public async Task Deleting_User_WithTournamentParticipant_ThrowsDbUpdateException()
        {
            await _fixture.ResetDatabaseAsync();
            var organizer = PersistenceTestData.CreateUser(firstName: "Tournament", lastName: "Organizer");
            var player = PersistenceTestData.CreateUser(firstName: "Tournament", lastName: "Player");
            var tournament = PersistenceTestData.CreateTournament(organizer.Id, "DELETE-PTCP-USER");
            var participant = PersistenceTestData.CreateTournamentParticipant(tournament.Id, player.Id);

            await using (var dbContext = _fixture.CreateDbContext())
            {
                dbContext.Users.AddRange(organizer, player);
                dbContext.Tournaments.Add(tournament);
                dbContext.TournamentParticipants.Add(participant);
                await dbContext.SaveChangesAsync();
            }

            await using var deleteContext = _fixture.CreateDbContext();
            var persistedPlayer = await deleteContext.Users.SingleAsync(user => user.Id == player.Id);
            deleteContext.Users.Remove(persistedPlayer);

            var act = () => deleteContext.SaveChangesAsync();

            await act.Should().ThrowAsync<DbUpdateException>();
        }

        [DockerAvailableFact]
        public async Task Deleting_JassTable_WithPairing_ThrowsDbUpdateException()
        {
            await _fixture.ResetDatabaseAsync();
            var graph = await PersistPairingGraphAsync("DELETE-TABLE");

            await using var deleteContext = _fixture.CreateDbContext();
            var persistedTable = await deleteContext.JassTables.SingleAsync(table => table.Id == graph.Table.Id);
            deleteContext.JassTables.Remove(persistedTable);

            var act = () => deleteContext.SaveChangesAsync();

            await act.Should().ThrowAsync<DbUpdateException>();
        }

        [DockerAvailableFact]
        public async Task Deleting_Tournament_Cascades_ToRoundsPairingsParticipantsAndGames()
        {
            await _fixture.ResetDatabaseAsync();
            var graph = await PersistPairingGraphAsync("DELETE-TOURNAMENT", withGame: true, withPairingParticipants: true);

            await using (var deleteContext = _fixture.CreateDbContext())
            {
                var persistedTournament = await deleteContext.Tournaments.SingleAsync(
                    tournament => tournament.Id == graph.Tournament.Id);
                deleteContext.Tournaments.Remove(persistedTournament);
                await deleteContext.SaveChangesAsync();
            }

            await using var assertionContext = _fixture.CreateDbContext();
            (await assertionContext.Tournaments.CountAsync()).Should().Be(0);
            (await assertionContext.TournamentParticipants.CountAsync()).Should().Be(0);
            (await assertionContext.Rounds.CountAsync()).Should().Be(0);
            (await assertionContext.Pairings.CountAsync()).Should().Be(0);
            (await assertionContext.PairingParticipants.CountAsync()).Should().Be(0);
            (await assertionContext.Games.CountAsync()).Should().Be(0);
            (await assertionContext.JassTables.CountAsync()).Should().Be(1);
            (await assertionContext.Users.CountAsync()).Should().Be(graph.Players.Count + 1);
        }

        private async Task<PairingGraph> PersistPairingGraphAsync(
            string tournamentCode,
            bool withGame = false,
            bool withPairingParticipants = false)
        {
            var organizer = PersistenceTestData.CreateUser(firstName: "Tournament", lastName: "Organizer");
            var players = JassTournamentGraphBuilder.CreatePlayers(lastName: "Delete");
            var tournament = PersistenceTestData.CreateTournament(organizer.Id, tournamentCode);
            var participants = JassTournamentGraphBuilder.CreateParticipants(tournament.Id, players);
            var round = PersistenceTestData.CreateRound(tournament.Id);
            var table = PersistenceTestData.CreateJassTable(organizer.Id);
            var pairing = PersistenceTestData.CreatePairing(round.Id, table.Id);

            JassTournamentGraphBuilder.AddParticipantsToTournament(tournament, participants);
            var pairingParticipants = withPairingParticipants
                ? JassTournamentGraphBuilder.AddPairingParticipants(pairing, participants, players[0].Id)
                : [];
            var game = withGame
                ? JassTournamentGraphBuilder.AddPendingGame(pairing)
                : null;
            JassTournamentGraphBuilder.AddRoundWithPairing(tournament, round, pairing);

            await JassTournamentGraphBuilder.PersistGraphAsync(_fixture, organizer, players, tournament, table);

            return new PairingGraph(
                organizer,
                players,
                tournament,
                participants,
                round,
                table,
                pairing,
                pairingParticipants,
                game);
        }

        private sealed record PairingGraph(
            User Organizer,
            IReadOnlyCollection<User> Players,
            Tournament Tournament,
            IReadOnlyCollection<TournamentParticipant> Participants,
            Round Round,
            JassTable Table,
            Pairing Pairing,
            IReadOnlyCollection<PairingParticipant> PairingParticipants,
            Game? Game);
    }
}
