using JassTournamentManager.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace JassTournamentManager.Domain.Entities
{
    public class Tournament : Common.BaseEntity
    {
        private const TournamentStatus DefaultTournamentStatus = TournamentStatus.Active;

        private readonly List<Round> _rounds = [];
        private readonly List<TournamentParticipant> _participants = [];

        public Guid OrganizerId { get; private set; }

        public string Name { get; private set; } = string.Empty;

        public string? Location { get; private set; }

        public DateOnly Date { get; private set; }

        public string TournamentCode { get; private set; } = null!;

        public TournamentStatus Status { get; private set; }

        public TournamentConfig Config { get; private set; } = null!;

        public IReadOnlyCollection<Round> Rounds => _rounds.AsReadOnly();

        public IReadOnlyCollection<TournamentParticipant> Participants => _participants.AsReadOnly();

        private Tournament() { }

        public Tournament(
            Guid organizerId,
            string name,
            string? location,
            DateOnly date,
            string tournamenetCode,
            TournamentStatus status = DefaultTournamentStatus)
        {
            if (organizerId == Guid.Empty)
            {
                throw new ArgumentException("Organizer ID must not be empty.", nameof(organizerId));
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Name must not be empty.", nameof(name));
            }

            OrganizerId = organizerId;
            Name = name.Trim();
            Location = location?.Trim();
            Date = date;
            Status = status;
            TournamentCode = tournamenetCode;
        }

        public void Complete()
        {
            Status = TournamentStatus.Completed;
            MarkAsUpdated();
        }

        public void Cancel()
        {
            Status = TournamentStatus.Canceled;
            MarkAsUpdated();
        }

        public void UpdateDetails(string name, string? location, DateOnly date)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Name must not be empty.", nameof(name));
            }
            Name = name.Trim();
            Location = location?.Trim();
            Date = date;
            MarkAsUpdated();
        }

        public void SetConfig(TournamentConfig config)
        {
            if (Config is not null)
            {
                throw new InvalidOperationException("Tournament config has already been set.");
            }

            Config = config ?? throw new ArgumentNullException(nameof(Config));

            MarkAsUpdated();
        }
    }
}
