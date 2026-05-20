using JassTournamentManager.Domain.Common;

namespace JassTournamentManager.Domain.Entities
{
    public class JassTable : BaseEntity
    {
        private const bool DefaultIsActive = true;

        public Guid OrganizerId { get; private set; }

        public string Name { get; private set; } = string.Empty;

        public int DisplayOrder { get; private set; }

        public bool IsActive { get; private set; }

        private JassTable() { }

        public JassTable(Guid organizerId, string name, int displayOrder, bool isActive = DefaultIsActive)
        {
            VerifyArguments(organizerId, name, displayOrder);

            OrganizerId = organizerId;
            Name = name.Trim();
            DisplayOrder = displayOrder;
            IsActive = isActive;
        }

        public void Rename(string newName)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(newName);

            Name = newName.Trim();
            MarkAsUpdated();
        }

        public void ChangeDisplayOrder(int newDisplayOrder)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(newDisplayOrder);
            DisplayOrder = newDisplayOrder;
            MarkAsUpdated();
        }

        public void SetIsActive(bool isActive)
        {
            IsActive = isActive;
            MarkAsUpdated();
        }

        private static void VerifyArguments(Guid organizerId, string name, int displayOrder)
        {
            Guard.AgainstEmptyGuid(organizerId, nameof(organizerId));

            ArgumentException.ThrowIfNullOrWhiteSpace(name);
            Guard.AgainstMaxLength(name, 100, nameof(name));

            ArgumentOutOfRangeException.ThrowIfNegative(displayOrder);
        }
    }
}
