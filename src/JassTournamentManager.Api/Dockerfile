FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["src/JassTournamentManager.Api/JassTournamentManager.Api.csproj", "src/JassTournamentManager.Api/"]
COPY ["src/JassTournamentManager.Application/JassTournamentManager.Application.csproj", "src/JassTournamentManager.Application/"]
COPY ["src/JassTournamentManager.Contracts/JassTournamentManager.Contracts.csproj", "src/JassTournamentManager.Contracts/"]
COPY ["src/JassTournamentManager.Domain/JassTournamentManager.Domain.csproj", "src/JassTournamentManager.Domain/"]
COPY ["src/JassTournamentManager.Infrastructure/JassTournamentManager.Infrastructure.csproj", "src/JassTournamentManager.Infrastructure/"]

RUN dotnet restore "src/JassTournamentManager.Api/JassTournamentManager.Api.csproj"

COPY . .

RUN dotnet publish "src/JassTournamentManager.Api/JassTournamentManager.Api.csproj" \
    --configuration Release \
    --output /app/publish \
    --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "JassTournamentManager.Api.dll"]