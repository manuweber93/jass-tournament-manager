using System;
using System.Collections.Generic;
using System.Text;

namespace JassTournamentManager.Domain.Enums
{
    public enum ScoreVisibility
    {
        AlwaysVisibleForEveryone = 0,
        HiddenDuringActiveTournament = 1,
        OrganizerOnly = 2,
    }
}
