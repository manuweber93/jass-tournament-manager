FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# copy csproj and restore
COPY ["JassTournamentManager.Api.csproj", "./"]
RUN dotnet restore "JassTournamentManager.Api.csproj"

# copy rest
COPY . ./
RUN dotnet publish "JassTournamentManager.Api.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish ./

ENV ASPNETCORE_URLS="http://+:5000"
EXPOSE 5000
ENTRYPOINT ["dotnet", "JassTournamentManager.Api.dll"]
