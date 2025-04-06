# System Patterns

## Architecture Overview
The Reina.MacCredy e-commerce application follows a standard ASP.NET Core MVC architecture with additional patterns to ensure maintainability, scalability, and separation of concerns.

## Key Design Patterns

### MVC Pattern
- **Models**: Represent the data structures and business entities
- **Views**: Handle the user interface and presentation logic
- **Controllers**: Manage user input, work with models, and select views to render

### Repository Pattern
- Abstracts data access logic from business logic
- Provides a collection-like interface for domain objects
- Enables easier unit testing through dependency injection
- Centralizes data access logic, reducing duplication

### Dependency Injection
- Core services registered in Program.cs
- Promotes loose coupling between components
- Facilitates testing and maintenance
- Supports the SOLID principles of software design

### Unit of Work
- Coordinates operations across multiple repositories
- Ensures consistency in database operations
- Manages transactions when necessary

### Error Handling Pattern
- Defensive programming with null checks for critical objects
- Use of null-conditional operators (?.) for safer property access
- Try-catch blocks for potentially problematic code sections
- Graceful fallbacks to default values when exceptions occur
- AJAX error handling for client-side operations
- Toast notifications for user feedback on actions

### Design System Pattern
- **Color System**: Coffee-themed palette with consistent variables (coffee-dark, coffee-medium, coffee-light, coffee-cream)
- **Typography System**: Consistent font families and sizes for headings and body text
- **Spacing System**: Standardized spacing values for margins and padding
- **Component Library**: Reusable UI elements with consistent styling
- **Icon System**: Consistent use of Bootstrap icons throughout the application
- **Animation System**: Subtle transitions and hover effects for interactive elements
- **Terminology System**: Consistent use of terms like "Order" rather than "Add to Cart" across the application

### UI Design Patterns
- **Card Pattern**: Consistent content containers for products and profile sections
- **Product Card Pattern**: Standardized cards with image, title, price, rating, and actions
- **Service Card Pattern**: Specialized cards for account services with icon and action button
- **Quick Action Pattern**: Immediate access to common actions (order, view details)
- **Badge Pattern**: Category indicators on product cards
- **Hover Effect Pattern**: Visual feedback on interactive elements with scale and shadow changes
- **Animation Pattern**: Subtle transitions and transforms for interactive elements
- **Notification Pattern**: Toast messages and fade-in alerts for action feedback
- **Hero Section Pattern**: Branded header area for content sections
- **Section Organization**: Clear separation of content types on pages
- **Responsive Grid System**: Adapts layout to different screen sizes
- **Modal Dialog Pattern**: Detailed views and customization options without page navigation
- **Option Selector Pattern**: Radio button groups for product customization
- **Avatar Upload Pattern**: Visual interface for profile image management
- **Form Organization Pattern**: Logical grouping of input fields with clear labels
- **Search and Filter Pattern**: Standardized layout for search input and sorting controls

## Component Relationships

### Data Flow
```
User Request → Controller → Service → Repository → Database
                  ↑                                  ↓
                  └─────────── View ←────────────────┘
```

### User Interaction Flow
```
User Action → Client-side Validation → AJAX Request → Server Processing
     ↑                                                      ↓
     └─────────── Visual Feedback ←────────────────────────┘
```

### Service Organization
- **Identity Services**: Handle authentication and authorization
- **Product Services**: Manage product catalog and inventory
- **Order Services**: Process orders and manage checkout
- **Shopping Cart Services**: Manage cart functionality
- **Admin Services**: Support administrative operations
- **Profile Services**: Manage user profile information and settings

## Database Schema
The application uses Entity Framework Core with a code-first approach, defining the following key entities:

- **User**: Customer accounts and authentication data
- **Product**: Item details, pricing, and inventory
- **ProductOption**: Customization options for products (sizes, extras, flavors)
- **Category**: Product categorization
- **Order**: Purchase transactions and status
- **OrderItem**: Individual items within an order
- **ShoppingCart**: Temporary storage for items before purchase
- **Review**: Customer feedback on products

## Areas and Modularity
The application is organized into distinct areas:

- **Main Application**: Customer-facing storefront
- **Admin Area**: Administrative dashboard and tools
- **Identity Area**: User authentication and account management

## Security Considerations
- ASP.NET Core Identity for authentication and authorization
- Role-based access control for administrative features
- Input validation and sanitization to prevent attacks
- HTTPS enforcement for secure communication
- Anti-forgery tokens for form submissions
- Proper null checking to prevent null reference exceptions

## Caching Strategy
- In-memory caching for frequently accessed data
- Entity Framework caching for query results
- Static asset caching with appropriate headers

## Performance Optimizations
- Asynchronous programming with async/await
- Efficient database queries and indexing
- Pagination for large result sets
- Lazy loading for related entities when appropriate
- Defensive coding to prevent application crashes
- Dynamic loading of cart data with AJAX to improve page load times
- Optimized image loading for product displays

## UI/UX Patterns
- **Visual Hierarchy**: Important elements emphasized through size, color, and position
- **Progressive Disclosure**: Complex options revealed as needed (e.g., product customization)
- **Consistent Navigation**: Predictable menu structure across the application
- **Visual Feedback**: Actions confirmed through animations and notifications
- **Mobile-First Design**: Ensuring usability on all device sizes
- **Quick Order Flow**: Streamlined process for frequent customers
- **Accessible Controls**: Clear labels and sufficient touch targets
- **Action Confirmation**: Visual feedback when actions are successful
- **Content Separation**: Clear visual distinction between different content sections
- **Responsive Adaptation**: UI elements that reorganize based on screen size
- **Card Grid Layout**: Consistent product display using responsive card grids
- **Detail Modal Pattern**: Quick access to additional information without page navigation
- **Customization Flow**: Step-by-step process for product options
- **Price Formatting**: Consistent currency display with thousands separators
- **Terminology Consistency**: Using "Order" consistently for all product addition actions 