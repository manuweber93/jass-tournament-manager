namespace JassTournamentManager.Infrastructure.Tests.Persistence
{
    public sealed class DockerAvailableFactAttribute : FactAttribute
    {
        public DockerAvailableFactAttribute()
        {
            if (!IsDockerEndpointConfigured())
            {
                Skip = "Docker is required for PostgreSQL integration tests but no Docker endpoint is available.";
            }
        }

        private static bool IsDockerEndpointConfigured()
        {
            var dockerHost = Environment.GetEnvironmentVariable("DOCKER_HOST");
            if (!string.IsNullOrWhiteSpace(dockerHost))
            {
                return true;
            }

            if (OperatingSystem.IsWindows())
            {
                return File.Exists(@"\\.\pipe\docker_engine");
            }

            return File.Exists("/var/run/docker.sock");
        }
    }
}
