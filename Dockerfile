FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o /app/publish

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app

# Install curl for healthchecks
RUN apt-get update && apt-get install -y curl

# Create a non-root user
RUN adduser --disabled-password --gecos "" appuser

# Copy published files from build stage
COPY --from=build /app/publish .

# Create directory for uploaded images
RUN mkdir -p /app/wwwroot/images && \
    chown -R appuser:appuser /app
USER appuser

# Environment variables - these can be overridden in docker-compose.yml
ENV ASPNETCORE_ENVIRONMENT=Production \
    DOTNET_RUNNING_IN_CONTAINER=true \
    DatabaseProvider=SqlServer

EXPOSE 80
EXPOSE 443

# Add healthcheck
HEALTHCHECK --interval=30s --timeout=3s --retries=3 \
  CMD curl -f http://localhost:80/health || exit 1

# Set the entry point
ENTRYPOINT ["dotnet", "Reina.MacCredy.dll"]
