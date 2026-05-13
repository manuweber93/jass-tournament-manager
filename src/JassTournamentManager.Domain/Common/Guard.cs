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

    }
}
