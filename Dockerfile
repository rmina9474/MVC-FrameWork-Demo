FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o /app/publish

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Install EF Core tools for migrations
RUN apt-get update && apt-get install -y curl

# Create a non-root user
RUN adduser --disabled-password --gecos "" appuser

# Copy published files from build stage
COPY --from=build /app/publish .

# Set ownership
RUN chown -R appuser:appuser /app
USER appuser

EXPOSE 80
EXPOSE 443

# Add healthcheck
HEALTHCHECK --interval=30s --timeout=3s --retries=3 \
  CMD curl -f http://localhost:80/health || exit 1

# Set the entry point
ENTRYPOINT ["dotnet", "Reina.MacCredy.dll"]
