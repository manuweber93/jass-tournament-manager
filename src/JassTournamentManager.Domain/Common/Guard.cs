namespace JassTournamentManager.Domain.Common
{
    public static class Guard
    {
        public static void AgainstEmptyGuid(Guid value, string parameterName)
        {
            if (value == Guid.Empty)
            {
                throw new ArgumentException($"{parameterName} must not be empty.", parameterName);
            }
        }

        public static void AgainstDefaultDateOnly(DateOnly value, string parameterName)
        {
            if (value == default)
            {
                throw new ArgumentException($"{parameterName} must not be the default date.", parameterName);
            }
        }

        public static void AgainstMaxLength(string value, int maxLength, string parameterName)
        {
            if (value.Length > maxLength)
            {
                throw new ArgumentOutOfRangeException($"{parameterName} must not exceed ${maxLength} characters.");
            }
        }

        public static void AgainstOptionalMaxLength(string? value, int maxLength, string parameterName)
        {
            if (value is not null)
            {
                AgainstMaxLength(value, maxLength, parameterName);
            }
        }

    }
}
