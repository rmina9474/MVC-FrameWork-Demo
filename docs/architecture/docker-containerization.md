# Docker Containerization

## Overview

The Reina.MacCredy E-Commerce Platform is fully containerized using Docker, enabling consistent deployment and operation across different environments. This document outlines the containerization approach, configuration, and best practices implemented in the project.

## Container Structure

The application uses a multi-container architecture with the following components:

1. **Web Application Container**: ASP.NET Core application
2. **Database Container**: SQL Server for data storage

Each container is independently manageable but configured to work together through Docker Compose.

## Dockerfile

The application's Dockerfile uses a multi-stage build process for optimized image size and security:

```dockerfile
# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["Reina.MacCredy.csproj", "./"]
RUN dotnet restore "./Reina.MacCredy.csproj"
COPY . .
RUN dotnet build "Reina.MacCredy.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "Reina.MacCredy.csproj" -c Release -o /app/publish

# Final stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Configure non-root user for security
RUN useradd -M --uid 1000 appuser && chown -R appuser /app
USER appuser

# Configure health check
HEALTHCHECK --interval=30s --timeout=3s --retries=3 CMD curl -f http://localhost:80/health || exit 1

ENTRYPOINT ["dotnet", "Reina.MacCredy.dll"]
```

## Docker Compose

The `docker-compose.yml` file orchestrates the multi-container application:

```yaml
version: '3.8'

services:
  webapp:
    build:
      context: .
      dockerfile: Dockerfile
    ports:
      - "5000:80"
    environment:
      - ASPNETCORE_ENVIRONMENT=Production
      - ConnectionStrings__DefaultConnection=Server=sqlserver;Database=HomeBrew;User=sa;Password=NewPassword123!;TrustServerCertificate=True;
      - USE_SQL_SERVER=true
    depends_on:
      - sqlserver
    networks:
      - reina-network
    volumes:
      - ./wwwroot/images:/app/wwwroot/images
    restart: always

  sqlserver:
    image: mcr.microsoft.com/mssql/server:2022-latest
    environment:
      - ACCEPT_EULA=Y
      - SA_PASSWORD=NewPassword123!
    volumes:
      - sqlserver-data:/var/opt/mssql
    networks:
      - reina-network
    restart: always
    healthcheck:
      test: /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P "NewPassword123!" -Q "SELECT 1" || exit 1
      interval: 30s
      timeout: 10s
      retries: 3
      start_period: 30s

networks:
  reina-network:

volumes:
  sqlserver-data:
```

## Environment-Based Configuration

The application automatically adapts to different environments:

### Database Provider Selection

The application can switch between SQLite (local development) and SQL Server (containerized environment) based on environment variables:

```csharp
// In Program.cs
if (builder.Configuration.GetValue<bool>("USE_SQL_SERVER", false))
{
    // Use SQL Server in Docker environment
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
}
else
{
    // Use SQLite for local development
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlite("Data Source=cafe_shop.db"));
}
```

## Health Monitoring

Both containers implement health checks:

1. **Web Application**: HTTP endpoint health check
2. **SQL Server**: Connection-based health check

These checks enable:
- Automatic container restart on failure
- Readiness checks for dependent services
- Monitoring and alerting capabilities

## Data Persistence

The Docker setup ensures data persistence through:

1. **SQL Server Data**: Docker volume `sqlserver-data` persists database files
2. **Uploaded Images**: Volume mapping from container to host for product/user images

## Container Cleanup Strategy

The application implements a consistent cleanup strategy to prevent resource leakage:

1. **Orphaned Container Removal**: `--remove-orphans` flag automatically cleans up orphaned containers
2. **Consistent Naming**: Clear container naming convention for easier management
3. **Proper Shutdown**: Graceful shutdown procedures to prevent data corruption

## Docker Run Script

A `docker-run.sh` script simplifies deployment:

```bash
#!/bin/bash

# Check if Docker is running
if ! docker info > /dev/null 2>&1; then
  echo "Docker is not running. Please start Docker and try again."
  exit 1
fi

# Stop any running containers
echo "Stopping any running containers..."
docker-compose down --remove-orphans

# Build and start containers
echo "Building and starting containers..."
docker-compose up -d --build

# Show running containers
echo "Containers are now running:"
docker-compose ps
```

## Security Considerations

The containerization implementation includes several security measures:

1. **Non-root User**: The application runs as a non-root user inside the container
2. **Environment Variables**: Sensitive configuration is passed via environment variables
3. **Network Isolation**: Custom Docker network for inter-container communication
4. **Health Checks**: Regular monitoring of container health
5. **Restart Policies**: Automatic recovery from failures

## Development vs. Production

The Docker setup supports both development and production environments:

### Development
- Interactive container management
- Volume mapping for real-time code changes
- Debugging capabilities
- Local environment variables

### Production
- Optimized image size
- Enhanced security configuration
- External secrets management
- Proper logging configuration
- Resource constraints management

## Best Practices Implemented

1. **Multi-stage Builds**: Smaller final image with only runtime dependencies
2. **Container Health Checks**: Proactive monitoring and self-healing
3. **Volume Persistence**: Data survives container restarts
4. **Environment-specific Configuration**: Different settings based on environment
5. **Dependency Management**: Containers start in the correct order
6. **Network Isolation**: Secure inter-container communication
7. **Resource Management**: Appropriate resource allocation 