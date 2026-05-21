using FluentAssertions;
using JassTournamentManager.Domain.Entities;
using JassTournamentManager.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace JassTournamentManager.Infrastructure.Tests.Persistence
{
    [Collection(PostgreSqlCollection.Name)]
    public class PersistenceRoundtripTests
    {
        private readonly PostgreSqlFixture _fixture;

        public PersistenceRoundtripTests(PostgreSqlFixture fixture)
        {
            _fixture = fixture;
        }

        [DockerAvailableFact]
        public async Task Can_Save_And_Load_User()
        {
            await _fixture.ResetDatabaseAsync();
            var user = PersistenceTestData.CreateSelfRegisteredUser(
                email: "player@example.com",
                firstName: "Jane",
                lastName: "Player");

            await using (var dbContext = _fixture.CreateDbContext())
            {
                dbContext.Users.Add(user);
                await dbContext.SaveChangesAsync();
            }

            await using var assertionContext = _fixture.CreateDbContext();
            var persistedUser = await assertionContext.Users.SingleAsync();

            persistedUser.Id.Should().Be(user.Id);
            persistedUser.Email.Should().Be(user.Email);
            persistedUser.FirstName.Should().Be(user.FirstName);
            persistedUser.LastName.Should().Be(user.LastName);
            persistedUser.SourceType.Should().Be(user.SourceType);
            persistedUser.IsActive.Should().Be(user.IsActive);
            persistedUser.IsSysAdmin.Should().Be(user.IsSysAdmin);
        }

        [DockerAvailableFact]
        public async Task Can_Save_And_Load_TournamentTemplate_With_ConfigValues()
        {
            await _fixture.ResetDatabaseAsync();
            var organizer = PersistenceTestData.CreateUser(firstName: "Tournament", lastName: "Organizer");
            var template = PersistenceTestData.CreateTournamentTemplate(organizer.Id);

            await using (var dbContext = _fixture.CreateDbContext())
            {
                dbContext.Users.Add(organizer);
                dbContext.TournamentTemplates.Add(template);
                await dbContext.SaveChangesAsync();
            }

            await using var assertionContext = _fixture.CreateDbContext();
            var persistedTemplate = await assertionContext.TournamentTemplates.SingleAsync();

            persistedTemplate.OrganizerId.Should().Be(organizer.Id);
            persistedTemplate.Location.Should().Be(template.Location);
            persistedTemplate.ConfigValues.Should().BeEquivalentTo(template.ConfigValues);
        }

        [DockerAvailableFact]
        public async Task Can_Save_And_Load_Tournament_With_Owned_ValueObjects()
        {
            await _fixture.ResetDatabaseAsync();
            var organizer = PersistenceTestData.CreateUser(firstName: "Tournament", lastName: "Organizer");
            var tournament = PersistenceTestData.CreateTournament(organizer.Id, "TOURNEY-1");

            await using (var dbContext = _fixture.CreateDbContext())
            {
                dbContext.Users.Add(organizer);
                dbContext.Tournaments.Add(tournament);
                await dbContext.SaveChangesAsync();
            }

            await using var assertionContext = _fixture.CreateDbContext();
            var persistedTournament = await assertionContext.Tournaments.SingleAsync();

            persistedTournament.OrganizerId.Should().Be(organizer.Id);
            persistedTournament.Status.Should().Be(tournament.Status);
            persistedTournament.Details.Should().BeEquivalentTo(tournament.Details);
            persistedTournament.ConfigValues.Should().BeEquivalentTo(tournament.ConfigValues);
        }

        [DockerAvailableFact]
        public async Task Can_Save_And_Load_Tournament_With_Participants_And_Rounds()
        {
            await _fixture.ResetDatabaseAsync();
            var seed = CreateTournamentSeed("TOURNEY-2");
            var participant = PersistenceTestData.CreateTournamentParticipant(
                seed.Tournament.Id,
                seed.Player.Id);
            var round = PersistenceTestData.CreateRound(seed.Tournament.Id);

            seed.Tournament.AddParticipant(participant);
            seed.Tournament.AddRound(round);

            await using (var dbContext = _fixture.CreateDbContext())
            {
                dbContext.Users.AddRange(seed.Organizer, seed.Player);
                dbContext.Tournaments.Add(seed.Tournament);
                await dbContext.SaveChangesAsync();
            }

            await using var assertionContext = _fixture.CreateDbContext();
            var persistedTournament = await assertionContext.Tournaments
                .Include(tournament => tournament.Participants)
                .Include(tournament => tournament.Rounds)
                .SingleAsync();

            persistedTournament.Participants.Should().ContainSingle()
                .Which.UserId.Should().Be(seed.Player.Id);
            persistedTournament.Rounds.Should().ContainSingle()
                .Which.RoundNumber.Should().Be(round.RoundNumber);
            persistedTournament.ConfigValues.NumberOfRounds.Should().Be(seed.Tournament.ConfigValues.NumberOfRounds);
        }

        [DockerAvailableFact]
        public async Task Can_Save_And_Load_Round_With_Pairing_Participants_And_Games()
        {
            await _fixture.ResetDatabaseAsync();
            var graph = await PersistTournamentGraphAsync(withGameScore: false);

            await using var assertionContext = _fixture.CreateDbContext();
            var persistedRound = await assertionContext.Rounds
                .Include(round => round.Pairings)
                    .ThenInclude(pairing => pairing.Participants)
                .Include(round => round.Pairings)
                    .ThenInclude(pairing => pairing.Games)
                .SingleAsync();

            var persistedPairing = persistedRound.Pairings.Should().ContainSingle().Subject;
            persistedPairing.Id.Should().Be(graph.Pairing.Id);
            persistedPairing.Participants.Should().HaveCount(graph.PairingParticipants.Count);
            persistedPairing.Games.Should().ContainSingle()
                .Which.Score.Should().BeNull();
        }

        [DockerAvailableFact]
        public async Task Can_Save_And_Load_Game_WithoutScore()
        {
            await _fixture.ResetDatabaseAsync();
            var graph = await PersistTournamentGraphAsync(withGameScore: false);

            await using var assertionContext = _fixture.CreateDbContext();
            var persistedGame = await assertionContext.Games.SingleAsync();

            persistedGame.Id.Should().Be(graph.Game.Id);
            persistedGame.Status.Should().Be(graph.Game.Status);
            persistedGame.Score.Should().BeNull();
        }

        [DockerAvailableFact]
        public async Task Can_Save_And_Load_Game_WithScore()
        {
            await _fixture.ResetDatabaseAsync();
            var graph = await PersistTournamentGraphAsync(withGameScore: true);

            await using var assertionContext = _fixture.CreateDbContext();
            var persistedGame = await assertionContext.Games.SingleAsync();

            persistedGame.Id.Should().Be(graph.Game.Id);
            persistedGame.Status.Should().Be(graph.Game.Status);
            persistedGame.Score.Should().NotBeNull();
            persistedGame.Score!.TeamAPoints.Should().Be(graph.Game.Score!.TeamAPoints);
            persistedGame.Score.TeamBPoints.Should().Be(graph.Game.Score.TeamBPoints);
            persistedGame.Score.TeamAMatchBonusAchieved.Should().Be(graph.Game.Score.TeamAMatchBonusAchieved);
            persistedGame.Score.TeamBMatchBonusAchieved.Should().Be(graph.Game.Score.TeamBMatchBonusAchieved);
            persistedGame.Score.EnteredBy.Should().Be(graph.Game.Score.EnteredBy);
            persistedGame.Score.EnteredAt.Should().BeCloseTo(graph.Game.Score.EnteredAt, TimeSpan.FromSeconds(1));
        }

        private async Task<TournamentGraph> PersistTournamentGraphAsync(bool withGameScore)
        {
            var seed = CreateTournamentSeed(withGameScore ? "TOURNEY-3" : "TOURNEY-4");
            var players = JassTournamentGraphBuilder.CreatePlayers(lastName: "Test");
            var participants = JassTournamentGraphBuilder.CreateParticipants(seed.Tournament.Id, players);
            var round = PersistenceTestData.CreateRound(seed.Tournament.Id);
            var table = PersistenceTestData.CreateJassTable(seed.Organizer.Id);
            var pairing = PersistenceTestData.CreatePairing(round.Id, table.Id);
            var game = withGameScore
                ? PersistenceTestData.CreateCompletedGame(pairing.Id, participants[0].Id)
                : PersistenceTestData.CreateGame(pairing.Id);

            JassTournamentGraphBuilder.AddParticipantsToTournament(seed.Tournament, participants);
            var pairingParticipants = JassTournamentGraphBuilder.AddPairingParticipants(pairing, participants);
            JassTournamentGraphBuilder.AddGame(pairing, game);
            JassTournamentGraphBuilder.AddRoundWithPairing(seed.Tournament, round, pairing);

            await JassTournamentGraphBuilder.PersistGraphAsync(_fixture, seed.Organizer, players, seed.Tournament, table);

            return new TournamentGraph(
                seed.Organizer,
                players,
                seed.Tournament,
                participants,
                round,
                table,
                pairing,
                pairingParticipants,
                game);
        }

        private static TournamentSeed CreateTournamentSeed(string tournamentCode)
        {
            var organizer = PersistenceTestData.CreateUser(firstName: "Tournament", lastName: "Organizer");
            var player = PersistenceTestData.CreateUser(firstName: "Single", lastName: "Player");
            var tournament = PersistenceTestData.CreateTournament(organizer.Id, tournamentCode);

            return new TournamentSeed(organizer, player, tournament);
        }

        private sealed record TournamentSeed(
            User Organizer,
            User Player,
            Tournament Tournament);

        private sealed record TournamentGraph(
            User Organizer,
            IReadOnlyCollection<User> Players,
            Tournament Tournament,
            IReadOnlyList<TournamentParticipant> Participants,
            Round Round,
            JassTable Table,
            Pairing Pairing,
            IReadOnlyCollection<PairingParticipant> PairingParticipants,
            Game Game);
    }
}
