#!/bin/bash

# Display header
echo "==============================================="
echo "   Running HomeBrew Shop in Docker Containers  "
echo "==============================================="

# Check if Docker is running
if ! docker info > /dev/null 2>&1; then
  echo "Error: Docker is not running or not installed"
  echo "Please start Docker and try again"
  exit 1
fi

# Stop any existing containers with the same names
echo "Stopping any existing containers..."
docker stop reina-webapp reina-sqlserver 2>/dev/null

# Remove containers if they exist
echo "Removing existing containers..."
docker rm reina-webapp reina-sqlserver 2>/dev/null

# Build the containers
echo "Building Docker containers..."
docker-compose build

# Start the containers
echo "Starting Docker containers..."
docker-compose up -d --remove-orphans

# Check the status
echo "Container status:"
docker-compose ps

echo ""
echo "==============================================="
echo "  Application should be available at:          "
echo "  http://localhost:8080                        "
echo "  For SQL Server: localhost:1499               "
echo "    Username: sa                               "
echo "    Password: NewPassword123!                  "
echo "    Database: HomeBrew                         "
echo "==============================================="
echo ""
echo "To view logs: docker-compose logs -f"
echo "To stop: docker-compose down --remove-orphans" 