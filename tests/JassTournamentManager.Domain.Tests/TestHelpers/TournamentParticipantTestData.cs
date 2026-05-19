using JassTournamentManager.Domain.Entities;
using JassTournamentManager.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace JassTournamentManager.Domain.Tests.TestHelpers
{
    internal static class TournamentParticipantTestData
    {
        public static Guid CreateTournamentParticipantId() => Guid.NewGuid();

        public static RegistrationMethod CreateRegistrationMethod() => RegistrationMethod.ViaTournamentCode;

        public static TournamentParticipant CreateTournamentParticipant() =>
            new(
                TournamentTestData.CreateTournamentId(),
                UserTestData.CreateUserId(),
                CreateRegistrationMethod());

        public static TournamentParticipant CreateTournamentParticipant(Guid tournamentId) =>
            new(
                tournamentId,
                UserTestData.CreateUserId(),
                CreateRegistrationMethod());
    }
}
