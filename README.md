# E-Commerce Web Application - Reina.MacCredy Shop

This repository contains an ASP.NET Core e-commerce web application built using modern web development practices. The application provides a comprehensive online shopping experience with user management, product catalog, shopping cart functionality, secure payment processing, and an admin dashboard.

## 📋 Table of Contents

- [Project Overview](#project-overview)
- [Features](#features)
- [Technologies Used](#technologies-used)
- [Project Structure](#project-structure)
- [Development Workflow](#development-workflow)
- [How to Use This Project](#how-to-use-this-project)
- [Screenshots](#screenshots)
- [Setup and Installation](#setup-and-installation)
- [Docker Deployment](#docker-deployment)
- [Payment Gateway Integration](#payment-gateway-integration)
- [Security Features](#security-features)
- [Recent Updates](#recent-updates)
- [Known Issues](#known-issues)
- [Documentation](#documentation)
- [Contributing](#contributing)
- [License](#license)

## 🔍 Project Overview

This e-commerce application is a fully functional online shop with both customer-facing features and administrative tools. It follows modern ASP.NET Core MVC architecture with repository patterns, Entity Framework Core for data access, and Identity for user authentication and authorization. The application is containerized using Docker, allowing for easy deployment and scaling.

### Key Components:

- User-friendly product browsing and shopping experience
- Secure user authentication and account management
- Shopping cart and checkout process with multiple payment options
- Integrated payment gateways (MoMo, VNPay) with proper error handling
- Secure session management and data protection
- Admin dashboard for managing products, categories, and orders
- Order management and tracking
- Containerized deployment with Docker and Docker Compose
- Integrated health checks for monitoring

## ✨ Features

### Customer Features:
- **User Registration and Authentication**: Secure sign-up, login, and account management
- **Product Browsing**: View products with details, images, and pricing
- **Product Reviews**: Read and write reviews for products
- **Shopping Cart**: Add items, update quantities, and manage the cart
- **Checkout Process**: Complete orders with shipping information
- **Payment Processing**: Multiple payment gateways with robust error handling
- **Order History**: View past orders and order details
- **Quick Order**: Streamlined purchasing process for popular items
- **About Us Page**: Learn about the company's story and values
- **Locations Page**: Find store locations with detailed information and map

### Admin Features:
- **Admin Dashboard**: Overview of store statistics and recent orders
- **Product Management**: Add, edit, and delete products
- **Category Management**: Create and manage product categories
- **Order Management**: View and process customer orders
- **Sales Analytics**: Monitor sales trends and performance

### UI/UX Improvements:
- **Modern Interface**: Enhanced design with improved user experience
- **Interactive Product Cards**: Hover effects and quick action buttons
- **Visual Feedback**: Toast notifications for cart actions
- **Animation Effects**: Subtle animations for improved user engagement
- **Quantity Controls**: Intuitive interface for adjusting quantities in multiple locations
- **User-friendly Error Messages**: Clear feedback on payment processing issues
- **Coffee-themed Design System**: Consistent color palette and visual elements
- **Location Cards**: Visually engaging display of store locations with features and details
- **Values Display**: Visual presentation of company values with icons and hover effects

## 🛠️ Technologies Used

- **ASP.NET Core 8.0**: Modern web framework
- **Entity Framework Core 9.0.3**: ORM for database operations
- **ASP.NET Core Identity**: User authentication and authorization
- **ASP.NET Core Data Protection**: Secure session management
- **C# 12**: Primary programming language
- **HTML/CSS/JavaScript**: Frontend development
- **CSS Custom Properties**: Variables for consistent theming
- **CSS Animations**: Keyframes for interactive effects
- **Bootstrap 5**: Responsive UI framework
- **Bootstrap Icons**: Icon library for UI elements
- **SQL Server**: Database management
- **MVC Pattern**: Architectural pattern
- **Repository Pattern**: Data access abstraction
- **Dependency Injection**: For loosely coupled design
- **Docker & Docker Compose**: Containerization and orchestration
- **Health Checks**: Application monitoring
- **Newtonsoft.Json**: Session object serialization
- **MoMo Payment Gateway**: Payment processing
- **VNPay Payment Gateway**: Payment processing
- **HTTP Client**: External API communication with robust error handling
- **Google Maps**: Store location display

## 📂 Project Structure

The application follows a standard ASP.NET Core MVC structure with additional organization for better maintainability:

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

## 🔄 Development Workflow

The development of this project followed these key steps:

1. **Project Planning and Design**:
   - Defining requirements and features
   - Database schema design
   - UI/UX planning

2. **Project Setup**:
   - Creating ASP.NET Core project
   - Adding necessary packages and dependencies
   - Setting up Entity Framework and Identity

3. **Database Implementation**:
   - Creating models
   - Setting up database context
   - Implementing migrations

4. **Repository Pattern Implementation**:
   - Creating repository interfaces
   - Implementing data access logic
   - Setting up dependency injection

5. **Feature Development**:
   - Building product catalog and browsing
   - Implementing shopping cart functionality
   - Creating checkout process
   - Developing order management
   - Building admin dashboard and tools

6. **UI Modernization**:
   - Enhancing home page design
   - Implementing interactive product cards
   - Adding visual feedback mechanisms
   - Creating streamlined ordering experience
   - Applying coffee-themed design system

7. **Payment Gateway Integration**:
   - Implementing MoMo payment gateway
   - Implementing VNPay payment gateway
   - Creating robust error handling and fallback mechanisms
   - Adding proper HTTP client configuration and timeout handling

8. **Bug Fixing and Stability**:
   - Fixing controller inheritance issues
   - Resolving CSS syntax errors
   - Implementing proper error handling
   - Adding null checks for critical objects
   - Enhancing session state management

9. **Content Pages Development**:
   - Creating About Us page with company information
   - Implementing Locations page with store details and map
   - Ensuring consistent navigation and UI experience

10. **Docker Containerization**:
    - Creating Dockerfile for application
    - Setting up docker-compose.yml for orchestrating services
    - Configuring SQL Server database container
    - Implementing health checks for container monitoring
    - Ensuring data persistence with volumes

## 🚀 How to Use This Project

### Customer Experience

1. **Browsing Products**
   - Visit the home page to view featured products
   - Browse products by category using the navigation menu
   - Use the search function to find specific products by name or description
   - Click on any product to view its details, pricing, and customer reviews

2. **Quick Ordering**
   - Use the dedicated "Quick Order" section on the home page for faster purchasing of popular items
   - Adjust quantities directly on product cards
   - Add items to cart with a single click
   - Receive visual confirmation through toast notifications

3. **Account Management**
   - Register for a new account by clicking on "Register" in the navigation menu
   - Log in to your existing account from the "Login" page
   - Update your profile information from the account settings
   - View your order history under "My Orders" in your account dashboard

4. **Shopping Process**
   - Add products to your cart by clicking the "Add to Cart" button on product cards or detail pages
   - Adjust quantities or remove items from your cart on the Shopping Cart page
   - Proceed to checkout by clicking the "Checkout" button
   - Fill in your shipping details and select a payment method
   - Review your order and complete the purchase
   - View the order confirmation and receipt

5. **Payment Options**
   - Choose from multiple payment methods: Cash on Delivery, Credit Card, Bank Transfer, MoMo, or VNPay
   - For digital payments, you'll be redirected to the payment gateway to complete the transaction
   - After payment completion, you'll be redirected back to the order confirmation page
   - Clear error messages are displayed if any payment issues occur

6. **Product Reviews**
   - Leave reviews for products you've purchased
   - Rate products on a scale of 1-5 stars
   - Read other customers' reviews to help with purchasing decisions

7. **Learn About Us**
   - Visit the About Us page to learn about our company's story and values
   - Explore our coffee sourcing and roasting philosophy
   - Understand our commitment to quality and community

8. **Find Our Locations**
   - Visit the Locations page to find our coffee shops
   - View store details including address, hours, and available amenities
   - Use the interactive map to get directions
   - Learn about upcoming locations and expansion plans

### Admin Experience

1. **Accessing Admin Dashboard**
   - Log in with admin credentials
   - Navigate to the Admin dashboard through the admin menu dropdown or by going to /Admin/Dashboard

2. **Managing Products**
   - View all products in the admin product management section
   - Add new products with details, pricing, and images
   - Edit existing product information
   - Delete products that are no longer available
   - Sort and filter products for easier management

3. **Category Management**
   - Create new product categories
   - Edit existing categories
   - Organize products by assigning them to appropriate categories

4. **Order Management**
   - View all customer orders in the admin order section
   - Check order details including products, quantities, and customer information
   - Track order status and update it as needed
   - Print order invoices for record-keeping

5. **Sales Analytics**
   - View sales statistics on the admin dashboard
   - Monitor monthly revenue and popular products
   - Track customer purchasing trends

### User Roles and Permissions

- **Visitors (Unauthenticated Users)**
  - Browse products and view details
  - Read product reviews
  - Register for an account or log in

- **Registered Customers**
  - All visitor capabilities
  - Add products to cart and complete purchases
  - Leave reviews for purchased products
  - View order history and track current orders

- **Administrators**
  - All customer capabilities
  - Access to the admin dashboard
  - Full product and category management
  - Order processing and management
  - User account management
  - Sales data and analytics access

## ⚙️ Setup and Installation

### Prerequisites
- .NET 9.0 SDK (for local development)
- SQL Server (or SQL Server Express) (for local development)
- Visual Studio 2022 or Visual Studio Code (for local development)
- Docker and Docker Compose (for containerized deployment)
- SSL Certificate (for secure payment processing)
- Payment Gateway API Keys (for MoMo and VNPay integration)

### Installation Steps

#### Method 1: Local Development

1. **Clone the repository**
   ```
   git clone https://github.com/yourusername/Reina.MacCredy.git
   cd Reina.MacCredy
   ```

2. **Configure environment variables**
   
   Create a `.env` file in the root directory:
   ```
   ASPNETCORE_ENVIRONMENT=Development
   ASPNETCORE_URLS=https://localhost:5001;http://localhost:5000
   
   # Database Configuration
   ConnectionStrings__DefaultConnection=Server=YOUR_SERVER;Database=ReinaMacCredyShop;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True;
   
   # Session Configuration
   DataProtection__ApplicationName=Reina.MacCredy.Shop
   DataProtection__KeyLifetime=30.00:00:00
   
   # Payment Gateway Configuration
   Payment__MoMo__PartnerCode=your_partner_code
   Payment__MoMo__AccessKey=your_access_key
   Payment__MoMo__SecretKey=your_secret_key
   Payment__MoMo__ApiEndpoint=https://test-payment.momo.vn/v2/gateway/api/create
   
   Payment__VNPay__TmnCode=your_tmn_code
   Payment__VNPay__HashSecret=your_hash_secret
   Payment__VNPay__ApiEndpoint=https://sandbox.vnpayment.vn/paymentv2/vpcpay.html
   ```

3. **Update the connection string**
   
   In appsettings.json, verify the connection string matches your environment:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=YOUR_SERVER;Database=ReinaMacCredyShop;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True;"
     },
     "DataProtection": {
       "ApplicationName": "Reina.MacCredy.Shop",
       "KeyLifetime": "30.00:00:00"
     },
     "Payment": {
       "MoMo": {
         "PartnerCode": "",
         "AccessKey": "",
         "SecretKey": "",
         "ApiEndpoint": ""
       },
       "VNPay": {
         "TmnCode": "",
         "HashSecret": "",
         "ApiEndpoint": ""
       }
     }
   }
   ```

4. **Apply database migrations**
   ```
   dotnet ef database update
   ```

5. **Install SSL certificate for development**
   ```
   dotnet dev-certs https --clean
   dotnet dev-certs https --trust
   ```

6. **Run the application**
   ```
   dotnet run
   ```

## 🐳 Docker Deployment

The application is fully containerized and can be run using Docker and Docker Compose. This setup includes:

- An ASP.NET Core application container
- A SQL Server container
- Persistent volume for database storage
- Health checks for all containers
- Network configuration for container communication

### 1. Prerequisites for Docker Deployment

- Docker and Docker Compose installed on your system
- At least 4GB of RAM allocated to Docker (required for SQL Server)
- Free ports: 8080 (HTTP), 8443 (HTTPS), and 1499 (SQL Server)

### 2. Quick Start with Docker

The easiest way to run the application is using the provided script:

```bash
# Make the script executable (only needed once)
chmod +x docker-run.sh

# Run the application
./docker-run.sh
```

This script will:
- Check if Docker is running
- Stop and remove any existing containers with the same names
- Build the Docker images
- Start the containers
- Display the status and access information

### 3. Manual Docker Deployment

If you prefer to manage the Docker deployment manually:

1. **Configure environment variables**
   
   The project includes a `.env` file with default settings. You can modify this file or 
   create your own environment variables.

2. **Build and start the Docker containers**
   ```bash
   docker-compose build
   docker-compose up -d
   ```

3. **Access the application**
   - Web Application: http://localhost:8080
   - SQL Server: localhost:1499
     - Username: sa
     - Password: NewPassword123!
     - Database: HomeBrew

4. **Docker management commands**
   ```bash
   # View container status
   docker-compose ps
   
   # View logs
   docker-compose logs webapp
   docker-compose logs sqlserver
   
   # View logs in real-time
   docker-compose logs -f
   
   # Stop the containers
   docker-compose down
   
   # Restart after making changes
   docker-compose build --no-cache
   docker-compose up -d
   ```

### 4. Docker Configuration Files

The Docker setup consists of the following files:

- **Dockerfile**: Defines how the application image is built
- **docker-compose.yml**: Orchestrates the multi-container application
- **.dockerignore**: Specifies which files should not be included in the image
- **.env**: Contains environment variables for the Docker deployment

### 5. Database Persistence

The SQL Server data is stored in a persistent volume named `sqlserver-data`. This ensures your data survives container restarts.

### 6. Container Health Checks

Both the application and SQL Server containers have health checks configured:

- The application checks the `/health` endpoint every 30 seconds
- SQL Server verifies database connectivity with a simple query

These health checks help ensure the containers are running properly and allow Docker to restart them if they become unhealthy.

## 💳 Payment Gateway Integration

### MoMo Integration

The application integrates with the MoMo payment gateway using their REST API. Key implementation details:

1. **Configuration**
   - Partner Code, Access Key, and Secret Key are stored in application settings
   - API endpoint and request parameters are properly configured

2. **Implementation Highlights**
   - Properly configured HTTP client with timeout settings
   - Correct header management for API communication
   - HMAC-SHA256 signature generation for request authentication
   - Comprehensive exception handling for API communication errors
   - Fallback mechanisms for development/testing environments
   - User-friendly error messages with detailed logging

3. **MoMo Payment Flow**
   - Generate order reference ID and signature
   - Send payment request to MoMo API
   - Redirect user to MoMo payment page or QR code
   - Handle callback from MoMo after payment completion
   - Update order status based on payment result
   - Redirect user to order confirmation page

### VNPay Integration

Similar to MoMo, the application integrates with VNPay payment gateway:

1. **Configuration**
   - TMN Code and Hash Secret stored in application settings
   - API endpoint configured for development/production

2. **Implementation Details**
   - Secure hash generation for request authentication
   - Proper parameter formatting for API requests
   - Callback handling for payment verification
   - Order status updates based on payment results

### Security Considerations

- All payment gateway communication happens over HTTPS
- Sensitive credentials stored in secure application settings
- Proper signature/hash validation for incoming callbacks
- Database updates for payment status tracking
- Detailed logging for troubleshooting and auditing

## 🔒 Security Features

1. **Data Protection**
   ```csharp
   services.AddDataProtection()
       .SetApplicationName("Reina.MacCredy.Shop")
       .SetDefaultKeyLifetime(TimeSpan.FromDays(30));
   ```

2. **Session Security**
   ```csharp
   services.AddSession(options =>
   {
       options.Cookie.Name = ".Reina.MacCredy.Session";
       options.Cookie.HttpOnly = true;
       options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
       options.Cookie.SameSite = SameSiteMode.Strict;
       options.IdleTimeout = TimeSpan.FromMinutes(30);
   });
   ```

3. **Authentication/Authorization**
   - ASP.NET Core Identity for user management
   - Role-based access control for protected areas
   - Secure password policies

4. **Form Security**
   - Anti-forgery tokens for form submissions
   - Input validation and sanitization
   - CSRF protection

5. **API Security**
   - Secure communication with payment gateways
   - Proper signature validation for callbacks
   - Timeout configuration for HTTP clients

## 🆕 Recent Updates

### Docker Containerization
- Added complete Docker support with multi-container setup
  - Created properly configured Dockerfile for the application
  - Set up docker-compose.yml for orchestrating services
  - Implemented persistent volumes for database storage
  - Added health checks for container monitoring
  - Created deployment script for easy startup
- Improved application configuration for containerized environments
  - Made database provider selection environment-aware
  - Added environment variable support for all configurations
  - Ensured proper database initialization in containers
  - Added SQL Server health check integration

### Navigation and Information Pages
- Added complete About Us page with company story and values
  - Created visually engaging values section with icons
  - Added company history and coffee sourcing information
  - Applied coffee-themed design system consistently
- Implemented Locations page with store details and map
  - Created location card pattern for store information display
  - Added interactive map for easy navigation
  - Included store features and amenities using badge components
  - Added coming soon information for planned locations
- Fixed navigation menu links with proper routing
  - Added action methods to HomeController for new pages
  - Implemented active state highlighting for navigation items
  - Ensured consistent styling across all pages
- Streamlined About Us page by removing redundant team section
  - Focused content on company values and coffee sourcing
  - Enhanced visual presentation for better user experience

### MoMo Payment Gateway Fix
- Fixed HTTP header configuration in API requests
  - Removed incorrect `Content-Type` header from `DefaultRequestHeaders`
  - Added proper `Accept` header for API communication
  - Used `StringContent` constructor to properly set content type
- Added timeout configuration to prevent hanging requests
  - Set 30-second timeout for all HTTP client operations
- Enhanced error handling with specific exception types
  - Added dedicated handlers for HttpRequestException, TaskCanceledException
  - Improved JSON parsing with try-catch blocks
  - Updated order status in database for all error scenarios
- Improved response parsing with null-conditional operators
  - Added null checks for all API response properties
  - Implemented safe response handling with proper defaults
- Added detailed logging throughout payment flow
  - Logged request details, responses, and error information
  - Included correlation IDs in log entries for easier tracking

### Shopping Cart Improvements
- Enhanced session serialization with Newtonsoft.Json
- Implemented proper extension methods for session access
- Added TotalPrice property to ShoppingCart model
- Improved AJAX request handling with proper headers
- Enhanced error handling with detailed logging

### UI/UX Enhancements
- Added user-friendly error messages for payment failures
- Enhanced profile page design with avatar management
- Improved form validation with immediate feedback
- Added interactive animations and visual feedback
- Standardized button terminology across the application
- Implemented coffee-themed design system throughout the application
- Added hover effects to interactive elements for better engagement

## ⚠️ Known Issues
- Session serialization requires proper implementation of TotalPrice property
- Null reference exceptions in certain scenarios without proper null checking
- AJAX headers must include X-Requested-With for proper server identification
- Missing views for some controller actions cause runtime errors
- Button terminology inconsistency creates confusion in user experience
- Price formatting varies across different views
- Database connection occasionally fails on initial startup
- Better error handling needed for payment gateway communication
- Address validation could be improved for international addresses
- Some "Brew Haven" references still appear in various locations (rebranding in progress)
- Mobile image loading performance issues on slower connections
- UI inconsistencies between product listing and detail pages
- Form validation errors not clearly visible on mobile devices
- Admin product image uploads failing for large images

## 📚 Documentation

For comprehensive documentation, please refer to the `docs` directory:

### Architecture Documentation
- [Architecture Overview](docs/architecture/overview.md)
- [Data Model](docs/architecture/data-model.md)
- [Docker Containerization](docs/architecture/docker-containerization.md)

### API Documentation
- [Payment Integrations](docs/api/payment-integrations.md)

### Development Guides
- [Development Setup](docs/guides/development-setup.md)
- [Project Structure](docs/guides/project-structure.md)
- [Session Management](docs/guides/session-management.md)

The documentation provides detailed information about the system architecture, data model, development setup, and implementation details. Start with the [documentation index](docs/index.md) for a complete overview.

## 🤝 Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## 📝 License

This project is licensed under the MIT License - see the LICENSE file for details.

---

Developed by Reina MacCredy © 2025