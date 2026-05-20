using JassTournamentManager.Domain.Common;

namespace JassTournamentManager.Domain.ValueObjects
{
    public sealed record TournamentDetails
    {
        public string Name { get; init; } = string.Empty;

        public string? Location { get; init; }

        public DateOnly Date { get; init; }

        public string TournamentCode { get; init; } = null!;

        public TournamentDetails(
            string name,
            string? location,
            DateOnly date,
            string tournamentCode)
        {
            VerifyArguments(name, location, date, tournamentCode);

            Name = name.Trim();
            Location = location?.Trim();
            Date = date;
            TournamentCode = tournamentCode.Trim();
        }

        private static void VerifyArguments(string name, string? location, DateOnly date, string tournamentCode)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name);
            Guard.AgainstMaxLength(name, 200, nameof(name));

            Guard.AgainstOptionalMaxLength(location, 200, nameof(location));

            Guard.AgainstDefaultDateOnly(date, nameof(date));

            ArgumentException.ThrowIfNullOrWhiteSpace(tournamentCode);
            Guard.AgainstMaxLength(tournamentCode, 20, nameof(tournamentCode));
        }
    }
}
