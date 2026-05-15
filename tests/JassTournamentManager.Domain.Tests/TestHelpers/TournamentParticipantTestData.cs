using JassTournamentManager.Domain.Entities;
using JassTournamentManager.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace JassTournamentManager.Domain.Tests.TestHelpers
{
    internal static class TournamentParticipantTestData
    {
        public static TournamentParticipant CreateTournamentParticipant() =>
            new(
                TournamentTestData.CreateOrganizerId(),
                UserTestData.CreateUserId,
                RegistrationMethod.ViaTournamentCode);
    }
}
