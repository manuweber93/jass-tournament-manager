namespace JassTournamentManager.Application.Auth
{
    public static class PasswordPolicy
    {
        private const int MinimumPasswordLength = 8;

        public static bool IsValid(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                return false;
            }
            return password.Length >= MinimumPasswordLength
                && password.Any(char.IsDigit)
                && password.Any(IsSpecialCharacter);
        }
        private static bool IsSpecialCharacter(char character)
        {
            return !char.IsLetterOrDigit(character);
        }
    }
}
