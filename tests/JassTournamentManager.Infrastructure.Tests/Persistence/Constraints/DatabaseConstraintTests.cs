using FluentAssertions;
using JassTournamentManager.Domain.Entities;
using JassTournamentManager.Domain.Enums;
using JassTournamentManager.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace JassTournamentManager.Infrastructure.Tests.Persistence
{
    [Collection(PostgreSqlCollection.Name)]
    public class DatabaseConstraintTests
    {
        private readonly PostgreSqlFixture _fixture;

        public DatabaseConstraintTests(PostgreSqlFixture fixture)
        {
            _fixture = fixture;
        }

        [DockerAvailableFact]
        public async Task Saving_DuplicateUserEmail_ThrowsDbUpdateException()
        {
            await _fixture.ResetDatabaseAsync();
            var duplicateEmail = "duplicate@example.com";
            var firstUser = PersistenceTestData.CreateSelfRegisteredUser(duplicateEmail);
            var secondUser = PersistenceTestData.CreateSelfRegisteredUser(duplicateEmail, "Other", "User");

            await using var dbContext = _fixture.CreateDbContext();
            dbContext.Users.AddRange(firstUser, secondUser);

            var act = () => dbContext.SaveChangesAsync();

            await act.Should().ThrowAsync<DbUpdateException>();
        }

        [DockerAvailableFact]
        public async Task Saving_MultipleUsersWithoutEmail_IsAllowed()
        {
            await _fixture.ResetDatabaseAsync();
            var usersWithoutEmail = new[]
            {
                PersistenceTestData.CreateUser(firstName: "First", lastName: "Manual"),
                PersistenceTestData.CreateUser(firstName: "Second", lastName: "Manual"),
            };

            await using var dbContext = _fixture.CreateDbContext();
            dbContext.Users.AddRange(usersWithoutEmail);

            await dbContext.SaveChangesAsync();

            (await dbContext.Users.CountAsync()).Should().Be(usersWithoutEmail.Length);
        }

        [DockerAvailableFact]
        public async Task Saving_DuplicateTournamentCode_ThrowsDbUpdateException()
        {
            await _fixture.ResetDatabaseAsync();
            var organizer = PersistenceTestData.CreateUser(firstName: "Tournament", lastName: "Organizer");
            var duplicateTournamentCode = "DUPLICATE";
            var firstTournament = PersistenceTestData.CreateTournament(organizer.Id, duplicateTournamentCode);
            var secondTournament = PersistenceTestData.CreateTournament(organizer.Id, duplicateTournamentCode);

            await using var dbContext = _fixture.CreateDbContext();
            dbContext.Users.Add(organizer);
            dbContext.Tournaments.AddRange(firstTournament, secondTournament);

            var act = () => dbContext.SaveChangesAsync();

            await act.Should().ThrowAsync<DbUpdateException>();
        }

        [DockerAvailableFact]
        public async Task Saving_DuplicateTournamentParticipant_ThrowsDbUpdateException()
        {
            await _fixture.ResetDatabaseAsync();
            var seed = CreateTournamentWithPlayer("PARTICIPANT-1");
            var firstParticipant = PersistenceTestData.CreateTournamentParticipant(
                seed.Tournament.Id,
                seed.Player.Id);
            var secondParticipant = PersistenceTestData.CreateTournamentParticipant(
                seed.Tournament.Id,
                seed.Player.Id);

            await using var dbContext = _fixture.CreateDbContext();
            dbContext.Users.AddRange(seed.Organizer, seed.Player);
            dbContext.Tournaments.Add(seed.Tournament);
            dbContext.TournamentParticipants.AddRange(firstParticipant, secondParticipant);

            var act = () => dbContext.SaveChangesAsync();

            await act.Should().ThrowAsync<DbUpdateException>();
        }

        [DockerAvailableFact]
        public async Task Saving_DuplicateRoundNumber_InTournament_ThrowsDbUpdateException()
        {
            await _fixture.ResetDatabaseAsync();
            var seed = CreateTournamentWithPlayer("ROUND-1");
            var firstRound = PersistenceTestData.CreateRound(seed.Tournament.Id, roundNumber: 1);
            var secondRound = PersistenceTestData.CreateRound(seed.Tournament.Id, roundNumber: 1);

            await using var dbContext = _fixture.CreateDbContext();
            dbContext.Users.Add(seed.Organizer);
            dbContext.Tournaments.Add(seed.Tournament);
            dbContext.Rounds.AddRange(firstRound, secondRound);

            var act = () => dbContext.SaveChangesAsync();

            await act.Should().ThrowAsync<DbUpdateException>();
        }

        [DockerAvailableFact]
        public async Task Saving_DuplicatePairingTable_InRound_ThrowsDbUpdateException()
        {
            await _fixture.ResetDatabaseAsync();
            var graph = CreateRoundGraph("PAIRING-1");
            var firstPairing = PersistenceTestData.CreatePairing(graph.Round.Id, graph.Table.Id);
            var secondPairing = PersistenceTestData.CreatePairing(graph.Round.Id, graph.Table.Id);

            await using var dbContext = _fixture.CreateDbContext();
            dbContext.Users.Add(graph.Organizer);
            dbContext.Tournaments.Add(graph.Tournament);
            dbContext.Rounds.Add(graph.Round);
            dbContext.JassTables.Add(graph.Table);
            dbContext.Pairings.AddRange(firstPairing, secondPairing);

            var act = () => dbContext.SaveChangesAsync();

            await act.Should().ThrowAsync<DbUpdateException>();
        }

        [DockerAvailableFact]
        public async Task Saving_DuplicatePairingParticipant_InPairing_ThrowsDbUpdateException()
        {
            await _fixture.ResetDatabaseAsync();
            var graph = CreatePairingGraph("PAIRING-PTCP-1");
            var firstPairingParticipant = PersistenceTestData.CreatePairingParticipant(
                graph.Pairing.Id,
                graph.Participants[0].Id,
                Team.TeamA,
                graph.Participants[0].Id);
            var secondPairingParticipant = PersistenceTestData.CreatePairingParticipant(
                graph.Pairing.Id,
                graph.Participants[0].Id,
                Team.TeamB,
                graph.Participants[0].Id);

            await using var dbContext = _fixture.CreateDbContext();
            AddPairingGraph(dbContext, graph);
            dbContext.PairingParticipants.AddRange(firstPairingParticipant, secondPairingParticipant);

            var act = () => dbContext.SaveChangesAsync();

            await act.Should().ThrowAsync<DbUpdateException>();
        }

        [DockerAvailableFact]
        public async Task Saving_DuplicateGameNumber_InPairing_ThrowsDbUpdateException()
        {
            await _fixture.ResetDatabaseAsync();
            var graph = CreatePairingGraph("GAME-1");
            var firstGame = PersistenceTestData.CreateGame(graph.Pairing.Id, gameNumber: 1);
            var secondGame = PersistenceTestData.CreateGame(graph.Pairing.Id, gameNumber: 1);

            await using var dbContext = _fixture.CreateDbContext();
            AddPairingGraph(dbContext, graph);
            dbContext.Games.AddRange(firstGame, secondGame);

            var act = () => dbContext.SaveChangesAsync();

            await act.Should().ThrowAsync<DbUpdateException>();
        }

        [DockerAvailableFact]
        public async Task Saving_Tournament_With_MissingOrganizer_ThrowsDbUpdateException()
        {
            await _fixture.ResetDatabaseAsync();
            var tournament = PersistenceTestData.CreateTournament(Guid.NewGuid(), "MISSING-ORGANIZER");

            await using var dbContext = _fixture.CreateDbContext();
            dbContext.Tournaments.Add(tournament);

            var act = () => dbContext.SaveChangesAsync();

            await act.Should().ThrowAsync<DbUpdateException>();
        }

        [DockerAvailableFact]
        public async Task Saving_GameScore_With_MissingEnteredByParticipant_ThrowsDbUpdateException()
        {
            await _fixture.ResetDatabaseAsync();
            var graph = CreatePairingGraph("MISS-SCORE-BY");
            var game = PersistenceTestData.CreateCompletedGame(graph.Pairing.Id, Guid.NewGuid());

            await using var dbContext = _fixture.CreateDbContext();
            AddPairingGraph(dbContext, graph);
            dbContext.Games.Add(game);

            var act = () => dbContext.SaveChangesAsync();

            await act.Should().ThrowAsync<DbUpdateException>();
        }

        [DockerAvailableFact]
        public async Task Saving_PairingParticipant_With_MissingEnteredByParticipant_ThrowsDbUpdateException()
        {
            await _fixture.ResetDatabaseAsync();
            var graph = CreatePairingGraph("MISS-PAIR-BY");
            var pairingParticipant = PersistenceTestData.CreatePairingParticipant(
                graph.Pairing.Id,
                graph.Participants[0].Id,
                Team.TeamA,
                Guid.NewGuid());

            await using var dbContext = _fixture.CreateDbContext();
            AddPairingGraph(dbContext, graph);
            dbContext.PairingParticipants.Add(pairingParticipant);

            var act = () => dbContext.SaveChangesAsync();

            await act.Should().ThrowAsync<DbUpdateException>();
        }

        private static TournamentSeed CreateTournamentWithPlayer(string tournamentCode)
        {
            var organizer = PersistenceTestData.CreateUser(firstName: "Tournament", lastName: "Organizer");
            var player = PersistenceTestData.CreateUser(firstName: "Player", lastName: "One");
            var tournament = PersistenceTestData.CreateTournament(organizer.Id, tournamentCode);

            return new TournamentSeed(organizer, player, tournament);
        }

        private static RoundGraph CreateRoundGraph(string tournamentCode)
        {
            var seed = CreateTournamentWithPlayer(tournamentCode);
            var round = PersistenceTestData.CreateRound(seed.Tournament.Id);
            var table = PersistenceTestData.CreateJassTable(seed.Organizer.Id);

            return new RoundGraph(seed.Organizer, seed.Tournament, round, table);
        }

        private static PairingGraph CreatePairingGraph(string tournamentCode)
        {
            var roundGraph = CreateRoundGraph(tournamentCode);
            var players = Enumerable.Range(1, 4)
                .Select(index => PersistenceTestData.CreateUser(firstName: $"Player{index}", lastName: "Constraint"))
                .ToArray();
            var participants = players
                .Select(player => PersistenceTestData.CreateTournamentParticipant(
                    roundGraph.Tournament.Id,
                    player.Id))
                .ToArray();
            var pairing = PersistenceTestData.CreatePairing(roundGraph.Round.Id, roundGraph.Table.Id);

            return new PairingGraph(
                roundGraph.Organizer,
                players,
                roundGraph.Tournament,
                participants,
                roundGraph.Round,
                roundGraph.Table,
                pairing);
        }

        private static void AddPairingGraph(JassTournamentDbContext dbContext, PairingGraph graph)
        {
            dbContext.Users.Add(graph.Organizer);
            dbContext.Users.AddRange(graph.Players);
            dbContext.Tournaments.Add(graph.Tournament);
            dbContext.TournamentParticipants.AddRange(graph.Participants);
            dbContext.Rounds.Add(graph.Round);
            dbContext.JassTables.Add(graph.Table);
            dbContext.Pairings.Add(graph.Pairing);
        }

        private sealed record TournamentSeed(
            User Organizer,
            User Player,
            Tournament Tournament);

        private sealed record RoundGraph(
            User Organizer,
            Tournament Tournament,
            Round Round,
            JassTable Table);

        private sealed record PairingGraph(
            User Organizer,
            IReadOnlyList<User> Players,
            Tournament Tournament,
            IReadOnlyList<TournamentParticipant> Participants,
            Round Round,
            JassTable Table,
            Pairing Pairing);
    }
}
