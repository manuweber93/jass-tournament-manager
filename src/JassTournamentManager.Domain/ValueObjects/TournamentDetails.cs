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
            ArgumentException.ThrowIfNullOrWhiteSpace(name);
            Guard.AgainstDefaultDateOnly(date, nameof(date));
            ArgumentException.ThrowIfNullOrWhiteSpace(tournamentCode);

            Name = name.Trim();
            Location = location?.Trim();
            Date = date;
            TournamentCode = tournamentCode.Trim();
        }
    }
}
