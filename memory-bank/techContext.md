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

## Technologies Used

This section outlines the technology stack and development tools used in the MVC Framework Demo project.

### Core Technologies

| Technology | Version | Purpose |
|------------|---------|---------|
| ASP.NET Core | 9.0 | Web application framework |
| Entity Framework Core | 9.0.3 | Object-relational mapping |
| C# | 12 | Primary programming language |
| Razor Views | - | View templating engine |
| ASP.NET Core Identity | 9.0 | Authentication and authorization |
| ASP.NET Core Data Protection | 9.0 | Secure session management |
| SQL Server | 2022 | Primary database (production/Docker) |
| SQLite | 3 | Development database |
| Bootstrap | 5.1 | Front-end UI framework |
| JavaScript | ES6+ | Client-side scripting |
| jQuery | 3.6.0 | DOM manipulation and AJAX |
| Docker | 27.5.1 | Containerization platform |
| Docker Compose | 2.32.4 | Multi-container orchestration |

### Development Tools

| Tool | Version | Purpose |
|------|---------|---------|
| Visual Studio | 2022 | Primary IDE for Windows development |
| Visual Studio Code | Latest | Cross-platform development |
| .NET CLI | 9.0 | Command-line tools for .NET development |
| Git | Latest | Version control |
| GitHub | - | Source code repository |
| NuGet | Latest | Package management |
| npm | Latest | Front-end package management |
| Postman | Latest | API testing |
| Browser DevTools | - | Front-end debugging |
| SQL Server Management Studio | 19 | Database management for SQL Server |
| DB Browser for SQLite | Latest | Database management for SQLite |

### Key NuGet Packages

| Package | Version | Purpose |
|---------|---------|---------|
| Microsoft.AspNetCore.Identity.EntityFrameworkCore | 9.0.3 | Identity storage with EF Core |
| Microsoft.EntityFrameworkCore.SqlServer | 9.0.3 | SQL Server provider for EF Core |
| Microsoft.EntityFrameworkCore.Sqlite | 9.0.3 | SQLite provider for EF Core |
| Microsoft.EntityFrameworkCore.Tools | 9.0.3 | Migrations and scaffolding for EF Core |
| Microsoft.AspNetCore.Authentication.JwtBearer | 9.0.3 | JWT authentication |
| Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation | 9.0.3 | Runtime compilation of Razor views |
| Microsoft.Extensions.Logging | 9.0.3 | Logging infrastructure |
| Newtonsoft.Json | 13.0.3 | JSON serialization/deserialization |
| Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore | 9.0.3 | EF Core diagnostic pages |
| Microsoft.AspNetCore.Diagnostics.HealthChecks | 9.0.3 | Health checks support |
| System.Net.Http.Json | 9.0.0 | JSON support for HttpClient |
| Microsoft.Extensions.Http | 9.0.3 | HTTP client factory |
| Microsoft.AspNetCore.Session | 9.0.3 | Session state management |
| Microsoft.Extensions.Caching.Memory | 9.0.3 | In-memory caching |

### Front-End Libraries

| Library | Version | Purpose |
|---------|---------|---------|
| Bootstrap | 5.1.3 | Responsive UI framework |
| jQuery | 3.6.0 | DOM manipulation and AJAX |
| popper.js | 2.11.5 | Tooltip and popover positioning |
| FontAwesome | 6.0.0 | Icon library |
| Bootstrap Icons | 1.8.0 | Icon library |
| Toastr | 2.1.4 | Toast notifications |
| SweetAlert2 | 11.4.0 | Modal dialogs |
| Chart.js | 3.7.0 | Data visualization |

## Development Environment

### Local Development Setup

1. **Required Software**
   - .NET 9.0 SDK or later
   - Visual Studio 2022 or Visual Studio Code
   - Git
   - SQL Server or SQLite
   - Node.js and npm (for front-end asset management)

2. **Configuration**
   - Development environment uses SQLite by default for simplicity
   - Identity configuration uses SQLite storage in development
   - Environment variables stored in appsettings.Development.json
   - User Secrets used for sensitive information

3. **Connection Strings**
   - Development (SQLite): `Data Source=homebrew.db`
   - Production (SQL Server): `Server=sqlserver;Database=HomeBrew;User Id=sa;Password=NewPassword123!;TrustServerCertificate=True;`

### Docker Development Setup

1. **Required Software**
   - Docker Desktop 27.5+ (with Docker Engine and Docker Compose)
   - At least 4GB of RAM allocated to Docker (required for SQL Server)
   - Free ports: 8080 (HTTP), 8443 (HTTPS), and 1499 (SQL Server)

2. **Docker Configuration**
   - Dockerfile uses multi-stage build for optimized image creation
   - Docker Compose orchestrates web application and SQL Server containers
   - Volume mounting for database persistence and image storage
   - Environment variable configuration through .env file
   - Health checks for application and database monitoring
   - Automatic container cleanup with `--remove-orphans` flag

3. **Startup Script**
   - `docker-run.sh` provides a convenient way to start the containerized application
   - Checks if Docker is running before attempting to start containers
   - Handles cleanup of existing containers
   - Builds and starts containers with proper configuration
   - Displays status and access information for the running containers
   - Includes `--remove-orphans` flag to clean up any orphaned containers

## Deployment Architecture

### Docker Containerization

1. **Container Structure**
   - Web Application Container: ASP.NET Core app running on Linux-based .NET 9.0 image
   - SQL Server Container: Microsoft SQL Server 2022 for Linux
   - Network: Isolated bridge network for secure communication between containers
   - Volumes: Persistent storage for SQL Server data and uploaded images

2. **Resource Requirements**
   - Web Application: 1-2 CPU cores, 512MB-1GB RAM
   - SQL Server: 2+ CPU cores, 2GB+ RAM (3GB recommended)
   - Storage: 1GB for application, 5GB+ for database (depends on data volume)

3. **Networking**
   - Port 8080: Public HTTP access to the web application
   - Port 8443: Public HTTPS access to the web application (when configured)
   - Port 1499: SQL Server access (can be restricted to internal network in production)

4. **Health Monitoring**
   - Web Application: HTTP health check at `/health` endpoint
   - SQL Server: TCP/connection health check
   - Automatic container restart on failure
   - Proper dependency handling between containers

5. **Setup Commands**
   ```bash
   # Start the application
   ./docker-run.sh
   
   # View logs
   docker-compose logs -f
   
   # Stop the application
   docker-compose down --remove-orphans
   ```

### Production Deployment Considerations

1. **Cloud Deployment Options**
   - AWS Elastic Container Service (ECS)
   - Azure Container Instances (ACI)
   - Google Cloud Run
   - Kubernetes cluster (for more complex deployments)

2. **Database Considerations**
   - Use managed database service in production (AWS RDS, Azure SQL)
   - Configure database backups and point-in-time recovery
   - Implement database connection pooling
   - Set up proper indexing for performance

3. **Security Concerns**
   - Use Docker secrets or environment variables for sensitive information
   - Implement HTTPS with proper SSL/TLS certificates
   - Restrict container access to necessary ports only
   - Regular security scanning of container images
   - Implement proper authentication and authorization
   - Configure SQL Server with strong passwords and restricted access

4. **Scaling Strategy**
   - Horizontal scaling for web application containers
   - Vertical scaling for database (or use managed service with read replicas)
   - Load balancing for web application traffic
   - Consider Redis for distributed caching and session storage

## Security Architecture

### Authentication & Authorization

1. **ASP.NET Core Identity**
   - Custom user model with extended profile properties
   - Role-based authorization (Admin, User)
   - Email confirmation for account verification
   - Password policy enforcement

2. **Data Protection**
   - Application-specific data protection keys
   - Key rotation policy (30 days)
   - Secure cookie handling

3. **Session Security**
   - HttpOnly cookies
   - Secure cookie policy (HTTPS)
   - SameSite cookie restrictions
   - Session timeout (30 minutes)

### API Security

1. **Payment Gateway Communication**
   - HTTPS for all API communication
   - Proper header management
   - API key authentication
   - Request signing for integrity verification
   - Timeout configuration to prevent hanging requests
   - Comprehensive error handling with logging

2. **CSRF Protection**
   - Anti-forgery tokens for form submissions
   - Validation of tokens for all POST, PUT, DELETE requests

### Database Security

1. **Entity Framework Security**
   - Parameterized queries to prevent SQL injection
   - Sensitive data attributes for data protection
   - EF Core query filters for data isolation

2. **Production Database Recommendations**
   - Use managed service with automated backups
   - Enable encryption at rest
   - Network isolation through VPC/private networks
   - Strong authentication with minimal privilege accounts

## Integrations

### Payment Gateway Integrations

1. **MoMo Payment Gateway**
   - Integration with MoMo REST API
   - HMAC-SHA256 signature generation
   - Proper HTTP client configuration with headers and timeout
   - QR code payment flow
   - Callback URL handling for payment verification
   - Error handling and recovery mechanisms

2. **VNPay Payment Gateway**
   - Integration with VNPay API
   - Secure hash generation
   - Proper parameter formatting
   - Callback handling
   - Order status tracking

### External Services

1. **Google Maps Integration**
   - Embed API for store location maps
   - Custom styling for map appearance
   - Marker customization for store locations

## Development Practices

### Coding Standards

1. **C# Coding Standards**
   - Follow Microsoft's C# coding conventions
   - Use C# 12 features where appropriate
   - Async/await for asynchronous operations
   - Proper exception handling
   - Dependency injection for loose coupling
   - Repository pattern for data access

2. **Front-End Standards**
   - Modular JavaScript with namespacing
   - Progressive enhancement approach
   - Mobile-first responsive design
   - Accessible HTML (WCAG compliance)
   - BEM methodology for CSS naming

### Testing Practices

1. **Manual Testing**
   - Testing matrix for browser compatibility
   - Mobile device testing
   - Payment flow testing with sandbox environments
   - User acceptance testing

2. **Future Automated Testing**
   - Unit tests for business logic
   - Integration tests for data access
   - UI tests for critical workflows
   - API tests for payment gateway integrations

### Error Handling & Logging

1. **Centralized Error Handling**
   - Global exception handling middleware
   - Custom error pages for HTTP status codes
   - Structured logging with semantic information
   - Environment-specific error details

2. **Logging Strategy**
   - Development: Console and debug logging
   - Production: File-based logging with rotation
   - Critical errors: Email notifications
   - Payment operations: Detailed audit logging

## Technical Constraints

1. **Browser Compatibility**
   - Modern browsers (Chrome, Firefox, Safari, Edge)
   - Minimum Internet Explorer 11 support (with polyfills)
   - Mobile browsers (iOS Safari, Chrome for Android)

2. **Performance Requirements**
   - Page load time < 3 seconds on broadband
   - First contentful paint < 1.5 seconds
   - Time to interactive < 5 seconds on mobile

3. **Security Requirements**
   - PCI DSS compliance considerations for payment handling
   - GDPR compliance for user data
   - Regular security audits
   - HTTPS enforcement

4. **Accessibility Requirements**
   - WCAG 2.1 AA compliance goal
   - Keyboard navigation support
   - Screen reader compatibility
   - Sufficient color contrast 