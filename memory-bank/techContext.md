# Technical Context

## Technology Stack

### Backend
- **Framework**: ASP.NET Core 8.0
- **Language**: C# 12
- **ORM**: Entity Framework Core 9.0.3
- **Database**: Microsoft SQL Server
- **Authentication**: ASP.NET Core Identity
- **API Style**: MVC pattern with view-based endpoints
- **JSON Serialization**: Newtonsoft.Json for session state management

### Frontend
- **View Engine**: Razor Views
- **CSS Framework**: Bootstrap 5
- **JavaScript Libraries**: jQuery for AJAX and DOM manipulation
- **Icons**: Bootstrap Icons
- **Responsive Design**: Mobile-first approach

### Infrastructure
- **Containerization**: Docker with docker-compose for local development
- **Source Control**: Git
- **IDE**: Visual Studio 2022 / VS Code
- **Deployment Target**: Containerized application
- **Session Management**: ASP.NET Core Session with secure cookie configuration

## Development Setup

### Prerequisites
- .NET 9.0 SDK or later
- Docker Desktop (for containerized development)
- SQL Server (or SQL Server container)
- Git
- IDE of choice (Visual Studio 2022 or VS Code recommended)

### Local Development
1. Clone the repository
2. Run `dotnet restore` to restore NuGet packages
3. Update connection string in `appsettings.json` if needed
4. Run `dotnet ef database update` to apply migrations
5. Run `dotnet run` to start the application

### Docker Development
1. Clone the repository
2. Run `docker-compose up` to build and start the application and database
3. Access the application at http://localhost:5000

## Architecture

### Application Layers
1. **Presentation Layer**: MVC Controllers and Razor Views
2. **Service Layer**: Business logic services
3. **Repository Layer**: Data access abstraction
4. **Data Layer**: Entity Framework Core and SQL Server

### Design Patterns
1. **Repository Pattern**: Data access abstraction
2. **Dependency Injection**: Service registration and resolution
3. **MVC Pattern**: Model-View-Controller for web interface
4. **Options Pattern**: Configuration management
5. **Factory Pattern**: For creating complex objects

### Data Flow
1. User interacts with Views
2. Controllers handle requests
3. Services process business logic
4. Repositories interact with database
5. Results flow back to Views

## Project Structure

### Key Directories
- **/Controllers**: MVC controllers
- **/Models**: Domain models and ViewModels
- **/Views**: Razor views
- **/Areas**: Feature-specific areas (like Admin, Identity)
- **/Services**: Business logic services
- **/Repositories**: Data access interfaces and implementations
- **/Extensions**: Extension methods (including session extensions for cart)
- **/Utilities**: Helper classes and utilities

### Important Files
- **Program.cs**: Application startup and DI configuration
- **appsettings.json**: Application configuration
- **Dockerfile**: Docker configuration
- **docker-compose.yml**: Multi-container Docker configuration

## Key Technical Considerations

### Session Management
- Using ASP.NET Core's session middleware
- Cookie-based session with proper security settings
- Session data stored as JSON using Newtonsoft.Json
- Custom extension methods (GetObjectFromJson, SetObjectAsJson) for type-safe session access
- Proper error handling for session state changes

### Data Protection
- Configured with explicit application name and key lifetime
- Secure cookie settings with HttpOnly, SameSite, and Secure flags
- CSRF protection through antiforgery tokens
- Proper authorization checks

### AJAX Handling
- jQuery for AJAX requests
- Proper X-Requested-With header for identifying AJAX requests
- Comprehensive error handling
- JSON responses with standardized format
- Client-side logging for troubleshooting

### Shopping Cart Implementation
- Shopping cart stored in session
- Cart data serialized to JSON
- Proper model definition with TotalPrice property
- Idempotent operations for adding/removing items
- Price formatting standardized to VND currency

### Authentication & Authorization
- ASP.NET Core Identity for user management
- Role-based authorization (Admin and User roles)
- Secure password hashing and storage
- Email confirmation

### Database Schema
- Code-first approach with migrations
- Proper relationships and constraints
- Indexed columns for performance
- Nullable types where appropriate

### Error Handling
- Global exception handler
- Controller-level try-catch blocks for domain-specific errors
- User-friendly error messages
- Detailed logging for debugging
- Developer-friendly error details in development environment

### Performance Considerations
- Efficient database queries
- Proper use of async/await
- Caching where appropriate
- Bundling and minification for frontend assets
- Lazy loading when beneficial

### Security
- HTTPS enforcement
- Cross-site scripting (XSS) protection
- Cross-site request forgery (CSRF) protection
- SQL injection prevention through parameterized queries
- Proper authentication and authorization
- Input validation and sanitization

## Technical Decisions

### Why ASP.NET Core MVC?
- Strong typing and compile-time checks
- Rich ecosystem and libraries
- Integration with Entity Framework Core
- Built-in authentication and authorization
- Familiar to the development team

### Why Entity Framework Core?
- ORM with strong LINQ support
- Code-first approach allows for easier domain modeling
- Migration support for database versioning
- Integration with ASP.NET Core
- Reduced boilerplate data access code

### Why Newtonsoft.Json?
- Robust and widely-used serialization library
- Excellent handling of complex object graphs
- Customizable serialization settings
- Better compatibility with existing code
- Well-documented and maintained

### Why Repository Pattern?
- Abstraction of data access logic
- Easier unit testing through mocking
- Separation of concerns
- Consistent data access patterns
- Flexibility to change data source implementation

### Why Docker?
- Consistent development environment
- Easier onboarding for new developers
- Production-like environment locally
- Simplified deployment process
- Infrastructure as code

## Technologies Used

### Core Framework
- **ASP.NET Core 8.0**: Modern, cross-platform web framework
- **C# 12**: Primary programming language with latest features

### Data Access
- **Entity Framework Core 9.0.3**: ORM for database operations
- **Microsoft SQL Server**: Primary database system
- **EF Core Migrations**: Database schema management

### Authentication & Authorization
- **ASP.NET Core Identity**: User authentication and management
- **Identity UI**: Pre-built UI for identity operations

### Frontend
- **Razor Views**: Server-side rendering of HTML
- **HTML5/CSS3**: Web standards for structure and styling
- **CSS Custom Properties**: Variables for consistent theming
- **CSS Animations**: Keyframes for interactive effects and fade-ins
- **CSS Transforms**: Visual effects for user engagement
- **CSS Flexbox/Grid**: Advanced layout techniques
- **Media Queries**: Responsive design for different device sizes
- **JavaScript/jQuery**: Client-side interactivity and AJAX functionality
- **File Upload Handling**: Client-side image preview and server-side processing
- **Bootstrap 5**: Responsive UI framework
- **Bootstrap Icons**: Icon library for the UI
- **Toast Notifications**: User feedback system

### Error Handling
- **Server-side validation**: Model validation and error handling
- **Client-side validation**: Form validation with jQuery
- **Null checks**: Defensive coding for User.Identity and other critical objects
- **Null-conditional operators**: Safe property access with ?. operator
- **Try-catch blocks**: Exception handling for critical operations
- **Toast notifications**: User-friendly error/success messages

### Containerization
- **Docker**: Application containerization
- **Docker Compose**: Multi-container application orchestration

### DevOps
- **Health Checks**: Application monitoring and status reporting

## Development Environment

### Requirements
- **.NET 9.0 SDK**: Core development framework
- **Visual Studio 2022** or **Visual Studio Code**: IDE/editor
- **SQL Server** (or SQL Server Express): Database engine
- **Docker Desktop**: For containerized development and testing

### Database Connection
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER;Database=ReinaMacCredyShop;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True;"
}
```

## Project Structure
```
Reina.MacCredy/
├── Areas/                # Feature-specific areas
│   ├── Admin/            # Admin dashboard and functionality
│   └── Identity/         # User authentication and management
├── Controllers/          # Application controllers
├── Extensions/           # Custom extensions
├── Migrations/           # Database migrations
├── Models/               # Data models
├── Repositories/         # Data access repositories
├── Services/             # Business logic and service implementations
├── Views/                # UI templates
│   └── Shared/           # Shared layout and partial views
│   └── Account/          # User account management views
├── wwwroot/              # Static resources (CSS, JS, images)
│   ├── css/              # Stylesheets
│   ├── js/               # JavaScript files
│   └── images/           # Image assets
│       └── avatars/      # User profile images
├── Dockerfile            # Container definition for the web application
├── docker-compose.yml    # Multi-container application setup
└── Program.cs            # Application configuration and startup
```

## Technical Constraints

### Performance Considerations
- Response time should be under 500ms for most operations
- Database queries should be optimized for efficiency
- Static assets should be properly cached
- Asynchronous operations for potentially slow tasks
- Minimize DOM manipulations for better client-side performance
- Optimize image uploads and processing for profile avatars

### Security Requirements
- All user data must be encrypted at rest
- Passwords must be securely hashed using Identity best practices
- HTTPS must be enforced for all communications
- Input validation must be performed on all user inputs
- Authentication and authorization must be enforced for protected resources
- Proper null checks to prevent null reference exceptions
- File upload validation to prevent security vulnerabilities

### Stability Requirements
- Defensive programming practices throughout the codebase
- Graceful handling of unexpected conditions and errors
- Comprehensive logging of exceptions and errors
- Fallback strategies for critical functionality
- Mobile-friendly responsive design that works on all devices

### Scalability Needs
- Application should be designed to scale horizontally
- Database design should consider potential growth
- Stateless design where possible to support load balancing
- Component-based UI structure for reusability and maintenance

### Browser Compatibility
- Must support modern browsers (Chrome, Firefox, Safari, Edge)
- Responsive design for mobile and tablet devices
- Progressive enhancement for older browsers where possible

## Dependencies
The application relies on the following NuGet packages:

- Microsoft.AspNetCore.Identity.EntityFrameworkCore (9.0.3)
- Microsoft.AspNetCore.Identity.UI (9.0.3)
- Microsoft.EntityFrameworkCore (9.0.3)
- Microsoft.EntityFrameworkCore.Design (9.0.3)
- Microsoft.EntityFrameworkCore.SqlServer (9.0.3)

## Frontend Libraries
- jQuery 3.6.0
- Bootstrap 5.3.0
- Bootstrap Icons 1.8.0

## UI Implementation Techniques
- CSS Custom Properties for consistent theming
- Card-based design for product displays and profile sections
- Toast notifications for user feedback
- Modal dialogs for quick views
- Hover effects for interactive elements
- Quantity controls with intuitive interface
- CSS animations for UI feedback
- Advanced CSS selectors for styling without additional markup
- Defensive null checking with null-conditional operators for UI elements
- File upload preview with client-side JavaScript
- Responsive service cards that adapt to different screen sizes
- Fade-in animations for alerts and important notifications

## Deployment Strategy
- Docker-based deployment for consistent environments
- CI/CD pipeline for automated testing and deployment
- Health checks for monitoring application status
- Database migrations applied automatically during deployment 