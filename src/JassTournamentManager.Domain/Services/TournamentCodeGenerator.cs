using System.Security.Cryptography;
using System;
using System.Collections.Generic;
using System.Text;

namespace JassTournamentManager.Domain.Services
{
    public static class TournamentCodeGenerator
    {
        private const string AllowedChars = "ABCDEFGHJKLMNPQRSTUVWXYZ123456789";

        public static string GenerateTournamentCode(int length = 6)
        {
            if (length <= 0)
            {
                throw new ArgumentException("Length must be a positive integer.", nameof(length));
            }

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
