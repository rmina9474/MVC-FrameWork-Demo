# Technical Context

## Technologies Used

### Core Framework

- **ASP.NET Core 8.0**: Modern, cross-platform web framework
- **C# 12**: Primary programming language with latest features
- **ASP.NET Core Data Protection**: Session and cookie security

### Data Access

- **Entity Framework Core 9.0.3**: ORM for database operations
- **Microsoft SQL Server**: Primary database system
- **EF Core Migrations**: Database schema management
- **SQLite**: Development database option

### Authentication & Authorization

- **ASP.NET Core Identity**: User authentication and management
- **Identity UI**: Pre-built UI for identity operations
- **Cookie Authentication**: Secure session handling
- **Role-based Authorization**: Admin/User permission control

### Session Management

- **Data Protection**: Application-specific key management
- **Cookie Security**: Secure session cookie configuration
- **Session State**: Shopping cart and user state handling
- **Security Headers**: Cookie policy enforcement
- **Idle Timeout**: Session expiration management
- **Session Extensions**: Custom methods for object serialization
- **Non-persistent Session**: Security-focused session configuration
- **Object JSON Serialization**: Converting complex objects for storage

### Frontend

- **Razor Views**: Server-side rendering of HTML
- **HTML5/CSS3**: Web standards for structure and styling
- **CSS Custom Properties**: Variables for consistent theming
- **CSS Animations**: Keyframes for interactive effects
- **CSS Transforms**: Visual effects for engagement
- **CSS Flexbox/Grid**: Advanced layout techniques
- **Media Queries**: Responsive design support
- **JavaScript/jQuery**: Client-side interactivity
- **File Upload**: Image preview and processing
- **Bootstrap 5**: Responsive UI framework
- **Bootstrap Icons**: Icon library
- **Toast Notifications**: User feedback system
- **Button Simplification**: Consistent "Order" button implementation
- **Navigation Optimization**: Streamlined menu options
- **Modal Dialogs**: Quick view product details
- **Card Design Pattern**: Consistent content containers
- **CSS Transitions**: Smooth state changes and animations
- **FormatPrice Extension**: Consistent currency formatting

### Payment Processing

- **MoMo Integration**: Mobile wallet gateway
- **VNPay Integration**: Vietnam-based provider
- **Payment Callbacks**: Success/failure handling
- **Transaction Status**: Response code processing
- **Cancellation Flow**: User-initiated cancellations
- **Route Attributes**: Explicit callback routing
- **State Tracking**: Order status management
- **Parameter Validation**: Callback verification
- **Error Display**: User-friendly payment failure messaging
- **Redirect Handling**: Proper post-payment user flow

### Error Handling

- **Server-side validation**: Model validation
- **Client-side validation**: Form validation
- **Null checks**: Defensive coding
- **Null-conditional operators**: Safe access
- **Try-catch blocks**: Exception handling
- **Toast notifications**: User feedback
- **Payment errors**: Gateway error handling
- **Status codes**: Response verification
- **Session errors**: State validation
- **Route errors**: Path matching
- **Null coalescing operator**: Default value provision
- **Conditional rendering**: Handling missing data gracefully
- **Exception logging**: Capturing error details

### Containerization

- **Docker**: Application containerization
- **Docker Compose**: Multi-container orchestration
- **Environmental Configuration**: Environment-specific settings
- **Volume Mounting**: Persistent data storage

### DevOps

- **Health Checks**: Application monitoring
- **Git**: Version control system
- **.gitignore**: Proper exclusion patterns
- **CI/CD Preparation**: Deployment pipeline setup

## Development Environment

### Requirements

- **.NET 9.0 SDK**: Core development framework
- **Visual Studio 2022** or **Visual Studio Code**: IDE/editor
- **SQL Server**: Database engine
- **Docker Desktop**: Containerized development
- **Git**: Source control management

### Database Connection

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER;Database=ReinaMacCredyShop;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True;"
}
```

## Project Structure

```
Reina.MacCredy/
├── Areas/                # Feature areas
│   ├── Admin/           # Admin dashboard
│   └── Identity/        # User authentication
├── Controllers/         # Application controllers
├── Extensions/          # Custom extensions
│   └── SessionExtensions.cs  # Session helpers and FormatPrice
├── Migrations/          # Database migrations
├── Models/              # Data models
├── Repositories/        # Data access
├── Services/            # Business logic
├── Views/               # UI templates
│   ├── Shared/         # Shared layouts
│   ├── Account/        # Account management
│   ├── Order/          # Order views including History
│   └── Product/        # Product catalog views
├── wwwroot/            # Static resources
│   ├── css/           # Stylesheets
│   ├── js/            # JavaScript
│   └── images/        # Images
│       └── avatars/   # Profile images
├── Dockerfile          # Container definition
├── docker-compose.yml  # Container setup
└── Program.cs          # Application startup
```

## Technical Constraints

### Performance Considerations

- Response time under 500ms
- Optimized database queries
- Proper asset caching
- Asynchronous operations
- Minimal DOM manipulation
- Optimized image handling
- Efficient session state
- Route caching
- Streamlined ordering process
- Content delivery optimization
- Lazy loading where appropriate
- Pagination for large datasets

### Security Requirements

- Data encryption at rest
- Secure password hashing
- HTTPS enforcement
- Input validation
- Protected resources
- Null checking
- Session protection
- Cookie security
- Payment verification
- Route security
- Data protection key management
- Cross-site scripting prevention
- CSRF protection
- Secure header configuration

### Stability Requirements

- Defensive programming
- Graceful error handling
- Comprehensive logging
- Fallback strategies
- Mobile responsiveness
- Service integration
- Session reliability
- Route resilience
- Consistent UI terminology
- Database connection resilience
- Error boundary implementation
- Null reference protection

### Scalability Needs

- Horizontal scaling
- Efficient database design
- Stateless design
- Component-based UI
- Session distribution
- Route distribution
- Containerized deployment
- Microservices preparation
- Caching strategy

### Browser Compatibility

- Modern browser support
- Mobile/tablet support
- Progressive enhancement
- Session compatibility
- Route consistency
- Button behavior consistency
- CSS fallbacks
- JavaScript feature detection
- Responsive behaviors

## Dependencies

The application relies on:

- Microsoft.AspNetCore.Identity.EntityFrameworkCore (9.0.3)
- Microsoft.AspNetCore.Identity.UI (9.0.3)
- Microsoft.EntityFrameworkCore (9.0.3)
- Microsoft.EntityFrameworkCore.Design (9.0.3)
- Microsoft.EntityFrameworkCore.SqlServer (9.0.3)
- Microsoft.AspNetCore.DataProtection (9.0.3)
- Microsoft.EntityFrameworkCore.Sqlite (9.0.3)
- Newtonsoft.Json (13.0.3)

## Frontend Libraries

- jQuery 3.6.0
- Bootstrap 5.3.0
- Bootstrap Icons 1.8.0
- Font Awesome 6.0.0 (selected icons)

## UI Implementation

- CSS Custom Properties
- Card-based design
- Toast notifications
- Modal dialogs
- Hover effects
- Quantity controls
- CSS animations
- Advanced selectors
- Null checking
- File upload preview
- Service cards
- Fade-in animations
- Session feedback
- Payment progress
- Consistent button terminology
- Simplified navigation
- Responsive tables
- Form validation styling
- Error messaging
- Loading indicators
- FormatPrice extension
- Order history display

## Payment Integration

- Async processing
- Redirect-based flow
- Response mapping
- Order tracking
- Session storage
- Parameter validation
- Cancellation handling
- Route configuration
- Error handling
- User feedback
- Status verification
- Secure parameter passing

## Deployment Strategy

- Docker deployment
- CI/CD pipeline
- Health monitoring
- Database migrations
- Session management
- Route configuration
- Environment variables
- Secrets management
- Logging configuration
- Backup strategy
