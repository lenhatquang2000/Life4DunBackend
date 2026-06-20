# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copy solution and project files
COPY *.sln ./
COPY src/Life4DunBackend.Core/*.csproj ./src/Life4DunBackend.Core/
COPY src/Life4DunBackend.Infrastructure/*.csproj ./src/Life4DunBackend.Infrastructure/
COPY src/Life4DunBackend.Api/*.csproj ./src/Life4DunBackend.Api/
COPY tests/Life4DunBackend.Tests/*.csproj ./tests/Life4DunBackend.Tests/

# Restore dependencies
RUN dotnet restore

# Copy all source files
COPY src/ ./src/
COPY tests/ ./tests/

# Build and publish release
RUN dotnet publish src/Life4DunBackend.Api/Life4DunBackend.Api.csproj -c Release -o out

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .

# Configure port to 3636
ENV ASPNETCORE_HTTP_PORTS=3636
EXPOSE 3636

ENTRYPOINT ["dotnet", "Life4DunBackend.Api.dll"]
