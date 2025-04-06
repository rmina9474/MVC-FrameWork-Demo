# Development Setup Guide

This guide provides instructions for setting up the development environment for the Reina.MacCredy E-Commerce Platform.

## Prerequisites

Before starting, ensure you have the following installed:

- **.NET 9.0 SDK** or later
- **Docker Desktop** (for containerized development)
- **SQL Server** (or SQL Server Express) - Optional for local development without Docker
- **Git**
- **Visual Studio 2022** or **Visual Studio Code**

## Getting the Source Code

1. Clone the repository:
   ```bash
   git clone https://github.com/your-username/reina-maccready-shop.git
   cd reina-maccready-shop
   ```

## Local Development Setup (Non-Docker)

### 1. Restore Dependencies

```bash
dotnet restore
```

### 2. Database Setup

By default, the application uses SQLite for local development. The database file `cafe_shop.db` is included in the repository.

If you prefer using SQL Server, update the connection string in `appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=ReinaMacCredyShop;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True;"
  },
  "USE_SQL_SERVER": true
}
```

### 3. Database Migrations

Apply existing migrations to create the database schema:

```bash
dotnet ef database update
```

If you need to add a new migration:

```bash
dotnet ef migrations add [MigrationName]
dotnet ef database update
```

### 4. Run the Application

```bash
dotnet run
```

The application will be available at `https://localhost:5001` and `http://localhost:5000`.

## Docker Development Setup

### 1. Start Docker Desktop

Ensure Docker Desktop is running on your system.

### 2. Environment Configuration

The application includes a `.env` file with environment variables for Docker. Review and modify as needed:

```
# Database Configuration
DB_SERVER=sqlserver
DB_NAME=HomeBrew
DB_USER=sa
DB_PASSWORD=NewPassword123!
DB_PORT=1433

# Web Application
WEB_PORT=5000

# Environment
ASPNETCORE_ENVIRONMENT=Production
USE_SQL_SERVER=true

# Momo Payment Gateway
MOMO_PARTNER_CODE=MOMOIQA420180417
MOMO_ACCESS_KEY=SvDmj2cOTYZmQQ3H
MOMO_SECRET_KEY=PPuDXq1KowPT1ftR8DvlQTHhC03aul17

# VNPay Gateway
VNPAY_TERMINAL_ID=GW01
VNPAY_SECRET_KEY=VNPAYSECRETKEY
```

### 3. Run with Docker Compose

Use the provided `docker-run.sh` script to start the containers:

```bash
# Make the script executable (first time only)
chmod +x docker-run.sh

# Run the script
./docker-run.sh
```

Or run Docker Compose commands manually:

```bash
# Build and start containers
docker-compose up -d --build

# Stop containers
docker-compose down --remove-orphans
```

The application will be available at `http://localhost:5000`.

### 4. Container Management

View running containers:

```bash
docker-compose ps
```

View container logs:

```bash
docker-compose logs -f
```

### 5. Database Access

If you need to access the SQL Server database directly:

```bash
# Connect to the container
docker exec -it reina-maccready-shop_sqlserver_1 /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P NewPassword123!
```

## Visual Studio Development

If you're using Visual Studio 2022:

1. Open the `Reina.MacCredy.sln` solution file
2. Right-click the project in Solution Explorer and select "Set as Startup Project"
3. Press F5 to build and run the application with debugging

## Visual Studio Code Development

If you're using Visual Studio Code:

1. Open the project folder in VS Code
2. Install the C# extension if not already installed
3. Open the integrated terminal and run:
   ```bash
   dotnet build
   dotnet run
   ```
4. For debugging, use the .NET Core launch configuration (launch.json)

## Application Configuration

### Connection Strings

The application can use either SQLite or SQL Server based on environment configuration:

```csharp
// This code is in Program.cs
if (builder.Configuration.GetValue<bool>("USE_SQL_SERVER", false))
{
    // Use SQL Server
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
}
else
{
    // Use SQLite
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlite("Data Source=cafe_shop.db"));
}
```

### Environment-Specific Settings

- Development settings: `appsettings.Development.json`
- Production settings: `appsettings.json` and environment variables in Docker

## Initial Test Data

The repository includes SQL scripts for initializing test data:

- `sqlite_create.sql`: Schema and initial data for SQLite
- `create_db.sql`: Schema and initial data for SQL Server
- `add_coffee_products.sql`: Additional product data
- `update_drinks.sql`: Updated drink options

To apply these scripts to SQL Server:

```bash
# Connect to your SQL Server instance and run the scripts
sqlcmd -S YOUR_SERVER -E -i create_db.sql
sqlcmd -S YOUR_SERVER -E -i add_coffee_products.sql
sqlcmd -S YOUR_SERVER -E -i update_drinks.sql
```

For SQLite, the database file `cafe_shop.db` already contains this data.

## Default Accounts

The application is seeded with the following accounts:

- **Admin User**:
  - Email: admin@example.com
  - Password: Admin123!

- **Customer User**:
  - Email: user@example.com
  - Password: User123!

## Development Best Practices

1. **Code Style**: Follow the established C# and .NET coding conventions
2. **Database Changes**: Always create migrations for database changes
3. **Error Handling**: Use try-catch blocks for error handling and proper logging
4. **Asynchronous Programming**: Use async/await pattern for I/O operations
5. **Unit Testing**: Write unit tests for new features
6. **Documentation**: Update documentation for significant changes

## Troubleshooting

### Common Issues

1. **Database Connection Issues**
   - Check connection string in appsettings.json
   - Ensure SQL Server is running
   - Verify user permissions

2. **Docker Errors**
   - Ensure Docker Desktop is running
   - Check if ports are already in use
   - Use `docker-compose logs` to view detailed error messages
   - Use the `--remove-orphans` flag with docker-compose down to clean up properly

3. **Entity Framework Migrations**
   - Run `dotnet ef migrations script` to view the migration SQL
   - Check for pending migrations with `dotnet ef migrations list`

4. **Port Conflicts**
   - Change the port mapping in docker-compose.yml if you have conflicts

## Useful Commands

```bash
# Restore dependencies
dotnet restore

# Build the project
dotnet build

# Run the application
dotnet run

# Watch for file changes and restart automatically
dotnet watch run

# Generate EF Core migrations
dotnet ef migrations add [MigrationName]

# Apply migrations
dotnet ef database update

# Clean the solution
dotnet clean

# Run tests
dotnet test
``` 