# E-Commerce Web Application - Reina.MacCredy Shop

This repository contains an ASP.NET Core e-commerce web application built using modern web development practices. The application provides a comprehensive online shopping experience with user management, product catalog, shopping cart functionality, and an admin dashboard.

## 📋 Table of Contents

- [Project Overview](#project-overview)
- [Features](#features)
- [Technologies Used](#technologies-used)
- [Project Structure](#project-structure)
- [Development Workflow](#development-workflow)
- [How to Use This Project](#how-to-use-this-project)
- [Screenshots](#screenshots)
- [Setup and Installation](#setup-and-installation)
- [Recent Updates](#recent-updates)
- [Known Issues](#known-issues)
- [Contributing](#contributing)
- [License](#license)

## 🔍 Project Overview

This e-commerce application is a fully functional online shop with both customer-facing features and administrative tools. It follows modern ASP.NET Core MVC architecture with repository patterns, Entity Framework Core for data access, and Identity for user authentication and authorization. The application is containerized using Docker, allowing for easy deployment and scaling.

### Key Components:

- User-friendly product browsing and shopping experience
- Secure user authentication and account management
- Shopping cart and checkout process
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
- **Order History**: View past orders and order details
- **Quick Order**: Streamlined purchasing process for popular items

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

## 🛠️ Technologies Used

- **ASP.NET Core 8.0**: Modern web framework
- **Entity Framework Core 9.0.3**: ORM for database operations
- **ASP.NET Core Identity**: User authentication and authorization
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

## 📂 Project Structure

The application follows a standard ASP.NET Core MVC structure with additional organization for better maintainability:

```
Reina.MacCredy/
├── Areas/
│   ├── Admin/          # Admin dashboard and functionality
│   └── Identity/       # User authentication and management
├── Controllers/        # Application controllers
├── Extensions/         # Custom extensions
├── Migrations/         # Database migrations
├── Models/             # Data models
├── Repositories/       # Data access repositories
├── Views/              # UI templates
├── wwwroot/            # Static resources (CSS, JS, images)
├── Dockerfile          # Container definition for the web application
├── docker-compose.yml  # Multi-container application setup
└── Program.cs          # Application configuration and startup
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

7. **Bug Fixing and Stability**:
   - Fixing controller inheritance issues
   - Resolving CSS syntax errors
   - Implementing proper error handling
   - Adding null checks for critical objects

8. **Deployment**:
   - Configuration for production environment
   - Database migration in production
   - Final testing

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

5. **Product Reviews**
   - Leave reviews for products you've purchased
   - Rate products on a scale of 1-5 stars
   - Read other customers' reviews to help with purchasing decisions

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

### Installation Steps

#### Method 1: Local Development

1. **Clone the repository**
   ```
   git clone https://github.com/yourusername/Reina.MacCredy.git
   cd Reina.MacCredy
   ```

2. **Update the connection string**
   
   In appsettings.json, update the connection string to point to your SQL Server instance:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=YOUR_SERVER;Database=ReinaMacCredyShop;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True;"
   }
   ```

3. **Apply database migrations**
   ```
   dotnet ef database update
   ```

4. **Run the application**
   ```
   dotnet run
   ```

#### Method 2: Docker Deployment

1. **Clone the repository**
   ```
   git clone https://github.com/yourusername/Reina.MacCredy.git
   cd Reina.MacCredy
   ```

2. **Build and start the Docker containers**
   ```
   docker-compose build
   docker-compose up -d
   ```

3. **Access the application**
   - Web Application: http://localhost:8080
   - SQL Server: localhost:1499
     - Username: sa
     - Password: NewPassword123!
     - Database: Webbanhang

4. **Docker management commands**
   ```
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

5. **Container Health Checks**
   - The application includes health check endpoints at `/health`
   - Both the web application and SQL Server containers have configured health checks that monitor their status

### Docker Configuration

The application is containerized using Docker with the following components:

1. **Web Application Container**
   - Based on .NET 8.0 runtime
   - Exposes ports 80 (HTTP) and 443 (HTTPS)
   - Maps to host ports 8080 and 8443 respectively
   - Includes health monitoring

2. **SQL Server Container**
   - Uses SQL Server 2022 Express
   - Persistent data storage through Docker volumes
   - Exposes port 1433
   - Maps to host port 1499

The complete configuration can be found in the `docker-compose.yml` and `Dockerfile` in the repository root.

## 🆕 Recent Updates

### Fixes and Improvements
- Fixed UserController inheritance issues by properly inheriting from Controller class
- Resolved CSS syntax errors with @keyframes in the home page
- Fixed type comparison issues in the home page (string vs int comparison)
- Implemented enhanced product cards with hover effects and quick action buttons
- Added direct "Add to Cart" functionality from multiple locations
- Improved quantity selection UI for better user experience
- Added visual feedback with toast notifications for cart actions
- Added animation effects to improve user engagement
- Fixed null reference exceptions in _Layout.cshtml by adding proper null checks
- Added null-conditional operators for ViewContext.RouteData.Values to prevent crashes
- Implemented better error handling for cart count display

### New Features
- Added a dedicated "Quick Order" section for faster purchasing
- Added new coffee-themed product catalog
- Enhanced product cards with on-hover quick actions
- Added toast notifications for cart actions

## ⚠️ Known Issues
- Some views may still have inadequate null checking
- Database migrations need careful management to prevent data loss
- Product search performance could be optimized for larger catalogs
- Some admin pages need UI improvements for smaller screens

## 🤝 Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## 📝 License

This project is licensed under the MIT License - see the LICENSE file for details.

---

Developed by Reina MacCredy © 2025