# Project Structure Guide

This document provides an overview of the Reina.MacCredy E-Commerce Platform project structure, highlighting key components and their responsibilities.

## Overview

The application follows a standard ASP.NET Core MVC architecture with additional organization for better maintainability. The project is organized into distinct layers with clear separation of concerns.

## Directory Structure

```
Reina.MacCredy/
├── Areas/
│   ├── Admin/            # Admin dashboard and functionality
│   └── Identity/         # User authentication and management
├── Controllers/          # Application controllers
├── Extensions/           # Custom extensions
│   └── SessionExtensions.cs  # Session serialization helpers
├── Migrations/           # Database migrations
├── Models/               # Data models
├── Repositories/         # Data access repositories
├── Services/             # Business logic services
│   └── PaymentService.cs # Payment processing service
├── Views/                # UI templates
│   └── Shared/           # Shared layout and partial views
│   └── Account/          # User account management views
│   └── Home/             # Home, About, and Locations views
├── wwwroot/              # Static resources (CSS, JS, images)
│   ├── css/              # Stylesheets
│   ├── js/               # JavaScript files
│   └── images/           # Image assets
│       └── avatars/      # User profile images
├── Dockerfile            # Container definition for the web application
├── docker-compose.yml    # Multi-container application setup
└── Program.cs            # Application configuration and startup
```

## Key Components

### Areas

The application uses ASP.NET Core Areas to organize related functionality:

1. **Admin Area**: Admin dashboard and management tools
   - Controllers for product, category, and order management
   - Views for administrative interfaces
   - Authorization checks for admin access

2. **Identity Area**: User authentication and account management
   - User registration and login
   - Profile management
   - Password recovery
   - Role management

### Controllers

Controllers handle HTTP requests and orchestrate the application flow:

- **HomeController**: Homepage, About Us, and Locations
- **ProductController**: Product browsing and details
- **ShoppingCartController**: Cart management
- **OrderController**: Order processing and history
- **AccountController**: User account management
- **PaymentController**: Payment processing and callbacks
- **AdminController**: Administrative functionality

### Models

Models represent the data structures used throughout the application:

1. **Domain Models**: Entity Framework entities mapping to database tables
   - `Product.cs`: Product information
   - `Category.cs`: Product categories
   - `Order.cs`: Customer orders
   - `OrderItem.cs`: Individual items in an order
   - `ApplicationUser.cs`: Extended user identity class

2. **View Models**: Data structures optimized for views
   - `ProductViewModel.cs`: Presentation model for products
   - `CartViewModel.cs`: Shopping cart display model
   - `OrderViewModel.cs`: Order display model
   - `CheckoutViewModel.cs`: Data for checkout process

3. **Data Transfer Objects (DTOs)**: Used for API communication
   - `PaymentRequestDto.cs`: Payment gateway request data
   - `PaymentResponseDto.cs`: Payment gateway response data

### Repositories

Repositories abstract data access operations:

- **IProductRepository/ProductRepository**: Product data operations
- **ICategoryRepository/CategoryRepository**: Category data operations
- **IOrderRepository/OrderRepository**: Order data operations
- **IUserRepository/UserRepository**: User-related data operations

### Services

Services encapsulate business logic:

- **PaymentService**: Handles payment gateway integration
- **OrderService**: Order processing logic
- **EmailService**: Email notification handling
- **FileService**: File upload and management

### Views

Views are organized following the MVC pattern:

- **Shared/**: Layout and partial views used across the application
  - `_Layout.cshtml`: Main site layout
  - `_Navigation.cshtml`: Main navigation
  - `_LoginPartial.cshtml`: Login/logout controls
  - Components: Reusable UI components (e.g., product cards)

- **Home/**: General pages
  - `Index.cshtml`: Home page with featured products
  - `About.cshtml`: About the company
  - `Locations.cshtml`: Store locations

- **Product/**: Product-related views
  - `Browse.cshtml`: Product catalog
  - `Details.cshtml`: Product details

- **ShoppingCart/**: Cart-related views
  - `Index.cshtml`: Shopping cart page
  - `_CartSummary.cshtml`: Cart summary partial view

- **Order/**: Order-related views
  - `Checkout.cshtml`: Checkout form
  - `Confirmation.cshtml`: Order confirmation
  - `History.cshtml`: Order history

- **Account/**: User account views
  - `Login.cshtml`: Login page
  - `Register.cshtml`: Registration page
  - `Profile.cshtml`: User profile management

### Static Content (wwwroot)

The wwwroot directory contains static resources:

- **CSS**: Stylesheets
  - `site.css`: Main stylesheet
  - `admin.css`: Admin-specific styles
  - `bootstrap.min.css`: Bootstrap framework

- **JavaScript**: Client-side functionality
  - `site.js`: Main JavaScript file
  - `cart.js`: Shopping cart functionality
  - `product.js`: Product-related functionality
  - `payment.js`: Payment processing

- **Images**: Image assets
  - `products/`: Product images
  - `avatars/`: User profile images
  - `banners/`: Banner images
  - `icons/`: UI icons

### Configuration Files

- **Program.cs**: Application startup and services configuration
- **appsettings.json**: Application configuration
- **appsettings.Development.json**: Development-specific settings
- **Dockerfile**: Docker container configuration
- **docker-compose.yml**: Multi-container configuration

## Architectural Patterns

The project implements several architectural patterns:

1. **MVC (Model-View-Controller)**: Core architectural pattern
2. **Repository Pattern**: Data access abstraction
3. **Dependency Injection**: Service resolution
4. **Unit of Work**: Transaction management
5. **Options Pattern**: Configuration management
6. **Factory Pattern**: Object creation

## Dependency Flow

The application follows a clean dependency flow:

```
Controllers → Services → Repositories → Database
     ↓
    Views
```

- Controllers depend on services and repositories
- Services depend on repositories and other services
- Repositories depend on the database context
- Views receive data from controllers

## Extension Methods

Custom extension methods enhance functionality:

- **SessionExtensions**: Add type-safe session operations
  - `GetObjectFromJson<T>`: Retrieve complex objects from session
  - `SetObjectAsJson`: Store complex objects in session

- **HttpContextExtensions**: HTTP context helpers
  - `IsAjaxRequest`: Detect AJAX requests
  - `GetAbsoluteUrl`: Generate absolute URLs

## Middleware Configuration

The application uses several middleware components configured in Program.cs:

- **Authentication**: ASP.NET Core Identity
- **Authorization**: Role-based and policy-based security
- **Session**: User session management
- **Static Files**: Serving static content
- **Routing**: URL-to-controller mapping
- **Error Handling**: Custom error pages
- **Health Checks**: Application monitoring

## Database Context

The `ApplicationDbContext` class manages Entity Framework Core operations:

- Entity mappings and relationships
- Database configuration
- Seed data

## Recommended Development Approach

When working with this project structure:

1. **Controller Development**: Add new controllers to implement user interactions
2. **Repository Extensions**: Add repository methods for new data access needs
3. **Service Implementation**: Implement business logic in appropriate services
4. **View Creation**: Create views for new functionality
5. **Dependency Registration**: Register new services in Program.cs

## Common Development Tasks

### Adding a New Entity

1. Create the entity class in the Models directory
2. Add DbSet property to ApplicationDbContext
3. Create a migration with `dotnet ef migrations add EntityName`
4. Apply the migration with `dotnet ef database update`
5. Create repository interface and implementation
6. Register the repository in DI container (Program.cs)

### Adding a New Feature

1. Create/modify repository methods for data access
2. Implement business logic in service classes
3. Create controller actions to handle requests
4. Develop views for user interaction
5. Add client-side code as needed 