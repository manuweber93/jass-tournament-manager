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

    }
}
