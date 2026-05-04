using System;
using System.Collections.Generic;
using System.Text;

namespace JassTournamentManager.Domain.Common
{
    public abstract class BaseEntity
    {
        public Guid Id { get; protected set; } = Guid.NewGuid();

        public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; protected set; } = DateTime.UtcNow;

        public void MarkAsUpdated()
        {
            UpdatedAt = DateTime.UtcNow;
        }

    }
}
