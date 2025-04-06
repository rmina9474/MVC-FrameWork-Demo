# Developer Guide

This guide provides essential information for developers working on the Brew Haven Coffee Shop e-commerce platform.

## Development Environment Setup

### Prerequisites
- .NET 8.0 SDK or later
- SQL Server 2022 or later (or SQL Server Express)
- Visual Studio 2022 or later (recommended) or Visual Studio Code
- Docker Desktop (for containerized development)
- Git

### Local Development Setup

1. Clone the repository:
   ```bash
   git clone https://github.com/yourusername/brew-haven.git
   cd brew-haven
   ```

2. Restore dependencies:
   ```bash
   dotnet restore
   ```

3. Set up the database:
   ```bash
   dotnet ef database update
   ```

4. Run the application:
   ```bash
   dotnet run
   ```

### Docker Development

1. Build and run the Docker containers:
   ```bash
   docker-compose up -d
   ```

2. Access the application at `http://localhost:5000`

3. Stop the containers when done:
   ```bash
   docker-compose down
   ```

## Project Structure Overview

The application follows a standard ASP.NET Core MVC structure with additional patterns for maintainability:

```
Brew Haven/
├── Areas/                # Feature-specific sections
├── Controllers/          # Request handlers
├── Extensions/           # Custom extension methods
├── Migrations/           # Database migration scripts
├── Models/               # Data models and view models
├── Repositories/         # Data access layer
├── Services/             # Business logic layer
├── Views/                # UI templates
└── wwwroot/              # Static resources
```

## Code Style and Standards

### C# Coding Conventions

- Use PascalCase for class names, method names, and public members
- Use camelCase for local variables and parameters
- Use async/await for asynchronous operations
- Use meaningful and descriptive names
- Keep methods focused on a single responsibility
- Document public APIs with XML comments

### MVC Conventions

- Controllers should be thin, delegating business logic to services
- Use ViewModels to pass data to views, not domain models
- Keep views simple, minimize logic in Razor syntax
- Use partial views for reusable UI components
- Use tag helpers over HTML helpers when possible

### JavaScript Conventions

- Follow modern ES6+ conventions
- Use jQuery for DOM manipulation and AJAX
- Organize JS code by feature or page
- Use camelCase for function and variable names
- Implement client-side validation when appropriate

## Database Access

### Entity Framework Core

- Use repositories to encapsulate data access logic
- Leverage async methods for database operations
- Use migrations for schema changes
- Use LINQ for querying data

### Example Repository Pattern Usage

```csharp
// Using the repository pattern
public async Task<IActionResult> Index()
{
    var products = await _productRepository.GetAllAsync();
    return View(products);
}
```

## Feature Development Workflow

1. **Create a new branch**:
   ```bash
   git checkout -b feature/feature-name
   ```

2. **Implement the feature**:
   - Add any required models
   - Create or update repositories
   - Implement business logic in services
   - Add controller actions
   - Create or update views

3. **Test locally**:
   - Run the application and test the feature
   - Write and run unit tests

4. **Submit a pull request**:
   - Push your branch and create a PR
   - Ensure the PR description explains the feature

## Testing

### Unit Testing

- Target service and repository layers primarily
- Mock external dependencies
- Follow AAA pattern (Arrange, Act, Assert)
- Keep tests focused and simple

### Example Test

```csharp
[Fact]
public async Task GetProductById_ReturnsProduct_WhenProductExists()
{
    // Arrange
    var productId = 1;
    var mockProduct = new Product { Id = productId, Name = "Test Coffee" };
    _mockRepository.Setup(r => r.GetByIdAsync(productId)).ReturnsAsync(mockProduct);
    
    // Act
    var result = await _productService.GetProductByIdAsync(productId);
    
    // Assert
    Assert.NotNull(result);
    Assert.Equal(productId, result.Id);
    Assert.Equal("Test Coffee", result.Name);
}
```

## Common Tasks

### Adding a New Product Feature

1. Create or update the Product model
2. Update the DbContext and create a migration
3. Add repository methods for data access
4. Implement service methods for business logic
5. Create controller actions
6. Create or update views

### Adding User Authentication Features

1. Configure Identity services in Program.cs
2. Create or customize Identity models
3. Implement custom authentication logic if needed
4. Create or update account controller
5. Create or update account views

## Error Handling

- Use try-catch blocks for potentially problematic code
- Log exceptions using the built-in logging framework
- Return user-friendly error messages
- Use global exception handling for unhandled exceptions

## Performance Best Practices

- Use asynchronous programming with async/await
- Implement caching for frequently accessed data
- Optimize database queries and avoid N+1 problems
- Use pagination for large data sets
- Compress and minify static assets

## Security Considerations

- Always validate and sanitize user input
- Use anti-forgery tokens for forms
- Implement proper authorization checks
- Never expose sensitive information in views
- Keep dependencies updated

## UI Development

### Design System

The application uses a coffee-themed design system:

- Color palette: coffee-dark, coffee-medium, coffee-light, coffee-cream
- Bootstrap 5 for core components
- Custom styles in /wwwroot/css/

### UI Components

- Use the established card patterns for consistent product display
- Follow the badge pattern for category indicators
- Implement consistent quick action buttons with the "Order" terminology
- Use toast notifications for user feedback

## Debugging Tips

- Use built-in logging to track application behavior
- Configure different log levels for development and production
- Use the browser's developer tools for client-side debugging
- Set breakpoints in Visual Studio for server-side debugging

## Common Issues and Solutions

### Session Serialization

- Use the built-in SessionExtensions for complex objects
- Example: `HttpContext.Session.SetObject("Cart", cart);`

### Payment Processing

- Check PaymentService logs for transaction issues
- Verify MoMo API connection settings
- Use the sandbox environment for testing

### Repository Data Access

- Ensure proper disposal of DbContext with using statements
- Verify connection strings in appsettings.json
- Check migrations are properly applied

## Deployment

### Production Deployment Checklist

1. Update connection strings for production
2. Set environment variables for sensitive information
3. Run database migrations
4. Configure proper logging levels
5. Enable HTTPS and configure certificates
6. Set up proper caching for static assets
7. Test the entire user flow before releasing

### Docker Deployment

Use the provided Dockerfile and docker-compose.yml for containerized deployment:

```bash
docker-compose -f docker-compose.production.yml up -d
```

## Contributing Guidelines

1. Follow the established code style and conventions
2. Write clean, maintainable, and testable code
3. Include appropriate comments and documentation
4. Write unit tests for new features
5. Keep pull requests focused on a single feature or fix

## Additional Resources

- [ASP.NET Core Documentation](https://docs.microsoft.com/en-us/aspnet/core)
- [Entity Framework Core Documentation](https://docs.microsoft.com/en-us/ef/core)
- [Bootstrap Documentation](https://getbootstrap.com/docs)
- [jQuery Documentation](https://api.jquery.com)
- Project-specific resources in the /docs directory 