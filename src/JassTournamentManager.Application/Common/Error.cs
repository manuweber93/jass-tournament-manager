namespace JassTournamentManager.Application.Common
{
    public sealed record Error
    {
        public string Code { get; }

        public string Message { get; }

        public ErrorType Type { get; }

        public Error(string code, string message, ErrorType type = ErrorType.Invalid)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(code);
            ArgumentException.ThrowIfNullOrWhiteSpace(message);

            Code = code;
            Message = message;
            Type = type;
        }
    }
}
