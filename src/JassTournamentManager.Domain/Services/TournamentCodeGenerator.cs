using System.Security.Cryptography;

namespace JassTournamentManager.Domain.Services
{
    public static class TournamentCodeGenerator
    {
        public const string AllowedChars = "ABCDEFGHJKLMNPQRSTUVWXYZ123456789";

        public static string GenerateTournamentCode(int length = 6)
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(length, 1);

            var codeChars = new char[length];

            for (int i = 0; i < length; i++)
            {
                int index = RandomNumberGenerator.GetInt32(AllowedChars.Length);
                codeChars[i] = AllowedChars[index];
            }

            return new string(codeChars);
        }
    }
}
