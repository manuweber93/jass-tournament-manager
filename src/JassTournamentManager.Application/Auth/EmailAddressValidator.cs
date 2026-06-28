using System.Net.Mail;

namespace JassTournamentManager.Application.Auth
{
    public static class EmailAddressValidator
    {
        public const int MaximumEmailLength = 320;
        public static bool IsValid(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return false;
            }

            string normalizedEmail = Normalize(email);
            if (normalizedEmail.Length > MaximumEmailLength)
            {
                return false;
            }

            try
            {
                var mailAddress = new MailAddress(normalizedEmail);
                if (!string.Equals(mailAddress.Address, normalizedEmail, StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }

                return HasValidDomainShape(mailAddress.Host);
            }
            catch
            {
                return false;
            }
        }

        public static string Normalize(string email)
        {
            return email.Trim().ToLowerInvariant();
        }

        private static bool HasValidDomainShape(string host)
        {
            return !string.IsNullOrWhiteSpace(host)
                && host.Contains('.', StringComparison.Ordinal)
                && !host.StartsWith(".", StringComparison.Ordinal)
                && !host.EndsWith(".", StringComparison.Ordinal);
        }
    }
}
