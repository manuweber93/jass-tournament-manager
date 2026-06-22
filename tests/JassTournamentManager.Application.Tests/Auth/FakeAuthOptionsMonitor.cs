using JassTournamentManager.Application.Auth;
using Microsoft.Extensions.Options;

namespace JassTournamentManager.Application.Tests.Auth
{
    internal sealed class FakeAuthOptionsMonitor : IOptionsMonitor<AuthOptions>
    {
        public AuthOptions CurrentValue { get; }

        public FakeAuthOptionsMonitor(AuthOptions authOptions)
        {
            CurrentValue = authOptions ?? throw new ArgumentNullException(nameof(authOptions));
        }

        public AuthOptions Get(string? name) => CurrentValue;

        public IDisposable OnChange(Action<AuthOptions, string?> listener)
        {
            return NullDisposable.Instance;
        }

        private sealed class NullDisposable : IDisposable
        {
            public static readonly NullDisposable Instance = new();

            public void Dispose() { }
        }
    }
}
