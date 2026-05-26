using JassTournamentManager.Domain.Entities;
using JassTournamentManager.Domain.Enums;

namespace JassTournamentManager.Infrastructure.Tests.Persistence
{
    internal static class JassTournamentGraphBuilder
    {
        public static User[] CreatePlayers(string lastName, int count = 4)
        {
            return Enumerable.Range(1, count)
                .Select(index => PersistenceTestData.CreateUser(
                    firstName: $"Player{index}",
                    lastName: lastName))
                .ToArray();
        }

        public static TournamentParticipant[] CreateParticipants(
            Guid tournamentId,
            IEnumerable<User> players)
        {
            return players
                .Select(player => PersistenceTestData.CreateTournamentParticipant(
                    tournamentId,
                    player.Id))
                .ToArray();
        }

        public static void AddParticipantsToTournament(
            Tournament tournament,
            IEnumerable<TournamentParticipant> participants)
        {
            foreach (var participant in participants)
            {
                tournament.AddParticipant(participant);
            }
        }

        public static PairingParticipant[] AddPairingParticipants(
            Pairing pairing,
            IReadOnlyList<TournamentParticipant> participants,
            Guid enteredByUserId)
        {
            var pairingParticipants = new[]
            {
                PersistenceTestData.CreatePairingParticipant(pairing.Id, participants[0].Id, Team.TeamA, enteredByUserId),
                PersistenceTestData.CreatePairingParticipant(pairing.Id, participants[1].Id, Team.TeamA, enteredByUserId),
                PersistenceTestData.CreatePairingParticipant(pairing.Id, participants[2].Id, Team.TeamB, enteredByUserId),
                PersistenceTestData.CreatePairingParticipant(pairing.Id, participants[3].Id, Team.TeamB, enteredByUserId),
            };

            foreach (var pairingParticipant in pairingParticipants)
            {
                pairing.AddParticipant(pairingParticipant);
            }

            return pairingParticipants;
        }

        public static void AddGame(Pairing pairing, Game game)
        {
            pairing.AddGame(game);
        }

        public static Game AddPendingGame(Pairing pairing)
        {
            var game = PersistenceTestData.CreateGame(pairing.Id);
            AddGame(pairing, game);

            return game;
        }

        public static void AddRoundWithPairing(Tournament tournament, Round round, Pairing pairing)
        {
            round.AddPairing(pairing);
            tournament.AddRound(round);
        }

        public static async Task PersistGraphAsync(
            PostgreSqlFixture fixture,
            User organizer,
            IEnumerable<User> players,
            Tournament tournament,
            JassTable table)
        {
            await using var dbContext = fixture.CreateDbContext();

            dbContext.Users.Add(organizer);
            dbContext.Users.AddRange(players);
            dbContext.Tournaments.Add(tournament);
            dbContext.JassTables.Add(table);
            await dbContext.SaveChangesAsync();
        }
    }
}
