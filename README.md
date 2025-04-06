# CoffeeShop Web Application

A modern ASP.NET Core MVC e-commerce application for a specialty coffee shop, featuring a clean architecture, responsive design, and a coffee-themed UI.

![CoffeeShop Web Application](wwwroot/images/coffee-logo.png)

## About the Project

CoffeeShop is a specialized e-commerce platform built using ASP.NET Core MVC architecture. The platform provides a modern shopping experience focused on coffee products with additional merchandise offerings. It follows clean architecture principles, implementing repository pattern, dependency injection, and MVC design patterns.

### Key Features

- **User Authentication**: Registration, login, profile management
- **Product Catalog**: Browsing, searching, filtering, detailed views
- **Shopping Cart**: Order functionality with quantity management
- **Checkout Process**: Address collection, payment options
- **Payment Integration**: MoMo and VNPay gateways
- **Order Management**: Order history, details
- **Admin Dashboard**: Product, order, and user management
- **Responsive Design**: Works on mobile, tablet, and desktop
- **Product Customization**: Size options, flavor additions, and extras

## Technology Stack

- **Backend**: ASP.NET Core 8.0, C# 12
- **Database**: Entity Framework Core 9.0.3, Microsoft SQL Server
- **Frontend**: HTML5, CSS3, JavaScript, Bootstrap 5
- **Authentication**: ASP.NET Core Identity
- **Containerization**: Docker
- **JSON Serialization**: Newtonsoft.Json

## Project Structure

The application follows a standard ASP.NET Core MVC architecture with:

- **Models**: Represent data structures and business entities
- **Views**: Handle user interface and presentation
- **Controllers**: Manage user input and application flow
- **Repositories**: Abstract data access from business logic
- **Services**: Handle business logic and operations

### Key Areas

- **Main Application**: Customer-facing storefront
- **Admin Area**: Administrative dashboard and tools
- **Identity Area**: User authentication and account management

## Getting Started

### Prerequisites

- .NET 8.0 SDK
- SQL Server (or SQL Server Express)
- Visual Studio 2022 or Visual Studio Code

### Installation

1. Clone the repository:
   ```
   git clone https://github.com/yourusername/MVC-FrameWork-Demo.git
   ```

2. Navigate to the project directory:
   ```
   cd MVC-FrameWork-Demo
   ```

3. Restore dependencies:
   ```
   dotnet restore
   ```

4. Update the database connection string in `appsettings.json`

5. Apply database migrations:
   ```
   dotnet ef database update
   ```

6. Run the application:
   ```
   dotnet run
   ```

7. Access the application at `https://localhost:5001`

## UI Design System

The application uses a consistent coffee-themed design system:

- **Color Palette**: 
  - Coffee Dark (#3C2A21)
  - Coffee Medium (#5C3D2E)
  - Coffee Light (#B85C38)
  - Coffee Cream (#E0C097)

- **UI Patterns**:
  - Card-based interface with hover effects
  - Consistent "Order" buttons (recently updated from "Add to Cart")
  - Modal dialogs for quick product actions
  - Toast notifications for user feedback

## Documentation

For more detailed documentation, check the `/docs` directory:

- [Architecture Overview](docs/architecture/overview.md)
- [User Guide](docs/guides/user-guide.md)
- [Developer Guide](docs/guides/developer-guide.md)
- [API Documentation](docs/api/README.md)

## Recent Updates

- Changed "Add to Cart" buttons to "Order" buttons throughout the application
- Redesigned the Menu page with modern UI elements
- Enhanced product customization options with radio button selectors
- Improved session state management
- Fixed payment processing errors with enhanced error handling

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Contact

For questions about the codebase, please contact the development team.