using System;
using System.Collections.Generic;
using System.Text;

namespace JassTournamentManager.Domain.Common
{
    public abstract class BaseEntity
    {
        public Guid Id { get; protected set; }

        public DateTime CreatedAt { get; protected set; }

        public DateTime UpdatedAt { get; protected set; }

        public BaseEntity()
        {
            DateTime now = DateTime.UtcNow;
            
            Id = Guid.NewGuid();
            CreatedAt = now;
            UpdatedAt = now;
        }

        public void MarkAsUpdated()
        {
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
