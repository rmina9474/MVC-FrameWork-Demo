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

### Session Management Pattern

- **Data Protection**: Explicit configuration with application name and key lifetime
- **Cookie Security**: Secure cookie settings with proper timeout and protection
- **Session State**: Proper handling of shopping cart and user state
- **Security Headers**: Appropriate security policies for cookies
- **Idle Timeout**: Configured session expiration handling
- **Essential Cookies**: Proper marking of required cookies
- **Non-sliding Expiration**: Fixed session lifetime for security
- **Object Serialization**: Custom extension methods for session storage

### Order Processing Pattern

- **Direct Ordering**: Streamlined process with one-click "Order" buttons
- **Cart Management**: Behind-the-scenes cart handling
- **Simplified Terminology**: Consistent "Order" action terminology
- **Feedback Messaging**: Toast notifications for order confirmations
- **Order History**: Comprehensive view of past orders
- **Unified Navigation**: Orders accessible through user profile dropdown
- **Price Formatting**: Consistent currency display with extension methods

### Payment Processing Pattern

- **Gateway Integration**: Separation of payment gateway specifics via service interfaces
- **Payment Service**: Centralized handling of payment processing logic
- **Response Code Mapping**: Standardized mapping of gateway-specific codes
- **Cancellation Flow**: Proper user experience for cancelled payments
- **Callback Routing**: Explicit route attributes for payment callbacks
- **Error Presentation**: Consistent messaging for failures
- **State Management**: Session-based order tracking
- **Verification Flow**: Robust callback parameter validation
- **Route Configuration**: Attribute-based routing for clear endpoint definition

### Error Handling Pattern

- Defensive programming with null checks
- Use of null-conditional operators
- Try-catch blocks for critical operations
- Graceful fallbacks for exceptions
- AJAX error handling
- Toast notifications for feedback
- Payment gateway error handling
- Status code verification
- Session state validation
- Route matching validation
- Null object pattern to prevent exceptions
- Conditional rendering to handle missing data

### Routing Pattern

- **Attribute Routing**: Explicit route definition for controllers
- **Convention-based Routes**: Default routing patterns
- **Area Routes**: Separate routing for admin area
- **Payment Callbacks**: Dedicated routes for gateway responses
- **Route Registration**: Proper order of route configuration
- **Route Constraints**: Validation of route parameters
- **Route Security**: Authentication and authorization checks
- **Action Terminology**: Consistent action naming in routes
- **MapControllers**: Explicit routing registration

### View Resolution Pattern

- **Shared Views**: Common views placed in Shared folder for broader accessibility
- **Controller Redirection**: Controllers redirect to a primary controller for shared views 
- **Explicit Path Views**: Controllers can specify the full path to a view when needed
- **Action Centralization**: Dedicated controller actions for specific views
- **View Fallback**: Looking for views in multiple locations
- **Standard Paths**: Following MVC conventions for view locations
- **View Action Routing**: Proper routing to specific view actions
- **OrderCompleted Pattern**: Central OrderCompleted action in OrderController with redirects from other controllers

### UI Design Patterns

- **Card Pattern**: Consistent content containers
- **Service Card Pattern**: Specialized action cards
- **Quick Action Pattern**: Immediate access buttons
- **Hover Effect Pattern**: Visual feedback
- **Notification Pattern**: Toast messages
- **Section Organization**: Clear content separation
- **Responsive Grid**: Adaptive layouts
- **Modal Dialog**: Detailed views
- **Avatar Upload**: Profile image handling
- **Form Organization**: Logical input grouping
- **Status Feedback**: Clear state indication
- **Session Feedback**: User state notifications
- **Button Consistency**: Unified "Order" terminology
- **Navigation Simplification**: Minimal, focused navbar options
- **Fade Animation**: Smooth transitions
- **Price Formatting**: Consistent currency display pattern

### Extension Method Pattern

- **Session Extensions**: Simplify session data handling
- **User Extensions**: Common user-related operations
- **View Extensions**: Reusable view components
- **Formatting Extensions**: Consistent data presentation
- **FormatPrice**: Standardized currency formatting method
- **Content Helpers**: Simplified content manipulation

## Component Relationships

### Data Flow

```
User Request → Controller → Service → Repository → Database
                  ↑                                  ↓
                  └─────────── View ←────────────────┘
```

### Session Flow

```
Request → Session Validation → Data Protection → Cookie Management
   ↑                                                    ↓
   └────────────── State Management ←──────────────────┘
```

### Order Flow

```mermaid
Product View → "Order" Button → Cart Addition → Checkout Process
    ↓                                              ↓
Feedback Notification ←────────────────── Order Confirmation
    ↓
Order History View (Past Orders)
```

### Payment Flow

```mermaid
Order → Payment Gateway → User Action (Pay/Cancel) → Callback Route
  ↓                                                      ↓
DB Update ← Payment Service ← Verification ← Response Processing
  ↓
User Feedback (Success/Failure/Cancellation)
```

### Service Organization

- **Identity Services**: Authentication and authorization
- **Product Services**: Catalog and inventory
- **Order Services**: Checkout processing
- **Shopping Cart Services**: Cart functionality
- **Payment Services**: Gateway integration
- **Session Services**: State management
- **Admin Services**: Administrative operations
- **Profile Services**: User information
- **Extension Services**: Reusable utility methods

## Database Schema

The application uses Entity Framework Core with a code-first approach, defining the following key entities:

- **User**: Customer accounts and authentication data
- **Product**: Item details, pricing, and inventory
- **Category**: Product categorization
- **Order**: Purchase transactions and status
- **OrderItem**: Individual items within an order
- **ShoppingCart**: Temporary storage for items before purchase
- **Review**: Customer feedback on products
- **ProductOption**: Customization options for products

## Areas and Modularity

The application is organized into distinct areas:

- **Main Application**: Customer-facing storefront
- **Admin Area**: Administrative dashboard and tools
- **Identity Area**: User authentication and account management

## Security Considerations

- ASP.NET Core Identity for authentication
- Role-based access control
- Input validation and sanitization
- HTTPS enforcement
- Anti-forgery tokens
- Proper null checking
- Session protection
- Cookie security
- Payment verification
- Route security
- Data protection keys
- Secure session configuration

## Caching Strategy

- In-memory caching
- Entity Framework caching
- Static asset caching
- Session state caching
- Response caching
- Content caching headers

## Performance Optimizations

- Asynchronous programming
- Efficient database queries
- Pagination for large sets
- Lazy loading when appropriate
- Defensive coding practices
- Dynamic cart loading
- Session state optimization
- Route caching
- Streamlined ordering process
- Pre-rendered content
- Static file optimization

## UI/UX Patterns

- **Visual Hierarchy**: Important elements emphasized
- **Progressive Disclosure**: Complex options revealed as needed
- **Consistent Navigation**: Predictable structure
- **Visual Feedback**: Action confirmation
- **Mobile-First**: Universal usability
- **Quick Order**: Streamlined process
- **Accessible Controls**: Clear interaction points
- **Action Confirmation**: Success feedback
- **Content Separation**: Clear sections
- **Responsive Adaptation**: Screen size handling
- **Error Communication**: Clear messaging
- **Recovery Path**: Clear next steps
- **Session Status**: State indication
- **Payment Progress**: Transaction feedback
- **Consistent Terminology**: Using "Order" across the application
- **Simplified Navigation**: Focused navbar without redundancy
- **Card-Based Layout**: Structured content presentation
- **Transition Effects**: Smooth state changes
- **Avatar Management**: User profile personalization
