FROM mcr.microsoft.com/dotnet/sdk:10.0 AS builder
WORKDIR /app
COPY SpotTrack.Platform/*.csproj SpotTrack.Platform/
RUN dotnet restore ./SpotTrack.Platform
COPY . .
RUN dotnet publish ./SpotTrack.Platform -c Release -o out
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=builder /app/out .
EXPOSE 8080
ENTRYPOINT ["dotnet", "SpotTrack.Platform.dll"]
