using JassTournamentManager.Domain.Common;
using JassTournamentManager.Domain.Enums;

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
            string tournamentCode,
            TournamentStatus status = DefaultTournamentStatus)
        {
            Guard.AgainstEmptyGuid(organizerId, nameof(organizerId));
            ArgumentException.ThrowIfNullOrWhiteSpace(name);
            ArgumentException.ThrowIfNullOrWhiteSpace(tournamentCode);

            OrganizerId = organizerId;
            Name = name.Trim();
            Location = location?.Trim();
            Date = date;
            Status = status;
            TournamentCode = tournamentCode.Trim();
        }

        public void SetConfig(TournamentConfig config)
        {
            if (Config is not null)
            {
                throw new InvalidOperationException("Tournament config has already been set.");
            }

            Config = config ?? throw new ArgumentNullException(nameof(config));
            MarkAsUpdated();
        }

        public void AddRound(Round round)
        {
            _rounds.Add(round);
            UpdateNumberOfRoundsInConfig();
            MarkAsUpdated();
        }

        public void RemoveRound(Guid roundId)
        {
            var roundToRemove = _rounds.FirstOrDefault(r => r.Id == roundId)
                ?? throw new ArgumentException($"Round with id {roundId} not found in tournament.");
            
            _rounds.Remove(roundToRemove);
            UpdateNumberOfRoundsInConfig();
            MarkAsUpdated();
        }

        public void AddParticipant(TournamentParticipant participant)
        {
            _participants.Add(participant);
            MarkAsUpdated();
        }

        public void RemoveParticipant(Guid participantId)
        {
            var participantToRemove = _participants.FirstOrDefault(p => p.Id == participantId)
                ?? throw new ArgumentException($"Participant with id {participantId} not found in tournament.");
            
            _participants.Remove(participantToRemove);
            MarkAsUpdated();
        }

        public void UpdateDetails(string name, string? location, DateOnly date)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name);

            Name = name.Trim();
            Location = location?.Trim();
            Date = date;
            MarkAsUpdated();
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

        public void Reactivate()
        {
            Status = TournamentStatus.Active;
            MarkAsUpdated();
        }

        private void UpdateNumberOfRoundsInConfig()
        {
            var UpdatedTournamentConfig = Config.ConfigValues with
            {
                NumberOfRounds = _rounds.Count
            };

            Config.UpdateConfig(UpdatedTournamentConfig);
        }
    }
}
