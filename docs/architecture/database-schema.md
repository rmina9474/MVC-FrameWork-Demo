# Database Schema Documentation

This document outlines the database schema for the Brew Haven Coffee Shop e-commerce platform.

## Entity Relationship Diagram

```
                    ┌──────────────┐
                    │     User     │
                    └──────┬───────┘
                           │
         ┌─────────────────┼─────────────────┐
         │                 │                 │
┌────────▼─────────┐    ┌──▼───────┐    ┌────▼────────┐
│      Order       │◄───┤ Review   │    │  Address    │
└────────┬─────────┘    └──────────┘    └─────────────┘
         │
┌────────▼─────────┐     ┌──────────────┐
│   Order Item     │◄────┤    Product   │◄─────┐
└──────────────────┘     └──────┬───────┘      │
                                │               │
                         ┌──────▼───────┐   ┌──▼───────────┐
                         │   Category   │   │Product Option│
                         └──────────────┘   └──────────────┘
```

## Database Tables

### User

Stores user account information and authentication details.

| Column Name      | Type         | Constraints          | Description                     |
|------------------|--------------|----------------------|---------------------------------|
| Id               | UNIQUEIDENTIFIER | PK              | Unique identifier               |
| UserName         | NVARCHAR(256)| NOT NULL, UNIQUE     | Username for login              |
| Email            | NVARCHAR(256)| NOT NULL, UNIQUE     | User's email address            |
| PasswordHash     | NVARCHAR(MAX)| NOT NULL             | Hashed password                 |
| FirstName        | NVARCHAR(100)| NOT NULL             | User's first name               |
| LastName         | NVARCHAR(100)| NOT NULL             | User's last name                |
| PhoneNumber      | NVARCHAR(20) |                      | Contact phone number            |
| CreatedAt        | DATETIME     | NOT NULL             | Account creation timestamp      |
| LastLoginAt      | DATETIME     |                      | Last login timestamp            |
| IsAdmin          | BIT          | NOT NULL, DEFAULT(0) | Admin status flag               |
| IsActive         | BIT          | NOT NULL, DEFAULT(1) | Account status                  |
| ReceiveMarketing | BIT          | NOT NULL, DEFAULT(0) | Marketing preferences           |

### Address

Stores shipping and billing addresses for users.

| Column Name     | Type            | Constraints        | Description                    |
|-----------------|-----------------|--------------------|-----------------------------|
| Id              | UNIQUEIDENTIFIER | PK                | Unique identifier           |
| UserId          | UNIQUEIDENTIFIER | FK                | Reference to User           |
| AddressType     | NVARCHAR(20)    | NOT NULL          | 'Shipping' or 'Billing'     |
| FirstName       | NVARCHAR(100)   | NOT NULL          | First name for the address  |
| LastName        | NVARCHAR(100)   | NOT NULL          | Last name for the address   |
| StreetAddress   | NVARCHAR(200)   | NOT NULL          | Street address              |
| City            | NVARCHAR(100)   | NOT NULL          | City                        |
| State           | NVARCHAR(50)    | NOT NULL          | State or province           |
| ZipCode         | NVARCHAR(20)    | NOT NULL          | Postal/ZIP code             |
| Country         | NVARCHAR(100)   | NOT NULL          | Country                     |
| IsDefault       | BIT             | NOT NULL          | Default address flag        |

### Category

Product categories for organization.

| Column Name  | Type            | Constraints        | Description                      |
|--------------|-----------------|-------------------|----------------------------------|
| Id           | INT             | PK, IDENTITY      | Unique identifier                |
| Name         | NVARCHAR(100)   | NOT NULL, UNIQUE  | Category name                    |
| Description  | NVARCHAR(500)   |                   | Category description             |
| ImageUrl     | NVARCHAR(255)   |                   | Category image URL               |
| IsActive     | BIT             | NOT NULL          | Category availability flag       |
| DisplayOrder | INT             | NOT NULL          | Sorting order for display        |

### Product

Stores product information.

| Column Name   | Type            | Constraints         | Description                      |
|---------------|-----------------|---------------------|----------------------------------|
| Id            | INT             | PK, IDENTITY        | Unique identifier                |
| Name          | NVARCHAR(100)   | NOT NULL            | Product name                     |
| Description   | NVARCHAR(1000)  |                     | Product description              |
| Price         | DECIMAL(10,2)   | NOT NULL            | Base product price               |
| ImageUrl      | NVARCHAR(255)   |                     | Product image URL                |
| CategoryId    | INT             | FK                  | Reference to Category            |
| InStock       | BIT             | NOT NULL            | Stock availability flag          |
| IsActive      | BIT             | NOT NULL            | Product availability             |
| CreatedAt     | DATETIME        | NOT NULL            | Product creation timestamp       |
| UpdatedAt     | DATETIME        | NOT NULL            | Last update timestamp            |
| Featured      | BIT             | NOT NULL, DEFAULT(0)| Featured product flag            |

### ProductOption

Stores customization options for products (sizes, extras, flavors).

| Column Name     | Type            | Constraints        | Description                        |
|-----------------|-----------------|--------------------|------------------------------------|
| Id              | INT             | PK, IDENTITY       | Unique identifier                  |
| ProductId       | INT             | FK                 | Reference to Product               |
| OptionName      | NVARCHAR(50)    | NOT NULL           | Option name (e.g., "Size")         |
| OptionType      | NVARCHAR(20)    | NOT NULL           | Option type (Choice, Boolean, etc.)|
| DisplayOrder    | INT             | NOT NULL           | Display ordering                   |
| IsRequired      | BIT             | NOT NULL           | Required option flag               |

### ProductOptionValue

Stores possible values for product options.

| Column Name     | Type            | Constraints        | Description                        |
|-----------------|-----------------|--------------------|------------------------------------|
| Id              | INT             | PK, IDENTITY       | Unique identifier                  |
| ProductOptionId | INT             | FK                 | Reference to ProductOption         |
| Value           | NVARCHAR(50)    | NOT NULL           | Option value (e.g., "Large")       |
| PriceAdjustment | DECIMAL(10,2)   | NOT NULL           | Price adjustment for this option   |
| DisplayOrder    | INT             | NOT NULL           | Display ordering                   |

### Order

Stores customer orders.

| Column Name       | Type            | Constraints        | Description                     |
|-------------------|-----------------|--------------------|---------------------------------|
| Id                | UNIQUEIDENTIFIER | PK                | Unique identifier               |
| OrderNumber       | NVARCHAR(50)    | NOT NULL, UNIQUE   | Human-readable order number     |
| UserId            | UNIQUEIDENTIFIER | FK                | Reference to User               |
| OrderDate         | DATETIME        | NOT NULL           | Order placement timestamp       |
| Status            | NVARCHAR(50)    | NOT NULL           | Order status                    |
| SubTotal          | DECIMAL(10,2)   | NOT NULL           | Order subtotal before tax       |
| TaxAmount         | DECIMAL(10,2)   | NOT NULL           | Tax amount                      |
| ShippingAmount    | DECIMAL(10,2)   | NOT NULL           | Shipping cost                   |
| TotalAmount       | DECIMAL(10,2)   | NOT NULL           | Total order amount              |
| PaymentMethod     | NVARCHAR(50)    | NOT NULL           | Payment method used             |
| PaymentStatus     | NVARCHAR(50)    | NOT NULL           | Payment status                  |
| ShippingAddressId | UNIQUEIDENTIFIER | FK                | Reference to Address            |
| BillingAddressId  | UNIQUEIDENTIFIER | FK                | Reference to Address            |
| Notes             | NVARCHAR(500)   |                    | Order notes                     |

### OrderItem

Stores individual items within an order.

| Column Name       | Type            | Constraints        | Description                     |
|-------------------|-----------------|--------------------|---------------------------------|
| Id                | UNIQUEIDENTIFIER | PK                | Unique identifier               |
| OrderId           | UNIQUEIDENTIFIER | FK                | Reference to Order              |
| ProductId         | INT             | FK                | Reference to Product            |
| ProductName       | NVARCHAR(100)   | NOT NULL          | Product name (snapshot)         |
| UnitPrice         | DECIMAL(10,2)   | NOT NULL          | Price per unit                  |
| Quantity          | INT             | NOT NULL          | Quantity ordered                |
| SubTotal          | DECIMAL(10,2)   | NOT NULL          | Line item subtotal              |
| Options           | NVARCHAR(MAX)   |                   | JSON of selected options        |

### ShoppingCart

Temporary storage for items before purchase.

| Column Name       | Type            | Constraints        | Description                     |
|-------------------|-----------------|--------------------|---------------------------------|
| Id                | UNIQUEIDENTIFIER | PK                | Unique identifier               |
| UserId            | UNIQUEIDENTIFIER | FK                | Reference to User               |
| SessionId         | NVARCHAR(100)   | NOT NULL          | Session ID for guest users      |
| CreatedAt         | DATETIME        | NOT NULL          | Cart creation timestamp         |
| UpdatedAt         | DATETIME        | NOT NULL          | Last update timestamp           |

### ShoppingCartItem

Items in a shopping cart.

| Column Name       | Type            | Constraints        | Description                     |
|-------------------|-----------------|--------------------|---------------------------------|
| Id                | UNIQUEIDENTIFIER | PK                | Unique identifier               |
| ShoppingCartId    | UNIQUEIDENTIFIER | FK                | Reference to ShoppingCart       |
| ProductId         | INT             | FK                | Reference to Product            |
| Quantity          | INT             | NOT NULL          | Quantity in cart                |
| Options           | NVARCHAR(MAX)   |                   | JSON of selected options        |
| AddedAt           | DATETIME        | NOT NULL          | Item addition timestamp         |

### Review

Customer feedback on products.

| Column Name       | Type            | Constraints        | Description                     |
|-------------------|-----------------|--------------------|---------------------------------|
| Id                | INT             | PK, IDENTITY       | Unique identifier               |
| ProductId         | INT             | FK                | Reference to Product            |
| UserId            | UNIQUEIDENTIFIER | FK                | Reference to User               |
| Rating            | INT             | NOT NULL          | Rating (1-5 stars)              |
| Comment           | NVARCHAR(1000)  |                   | Review text                     |
| CreatedAt         | DATETIME        | NOT NULL          | Review timestamp                |
| IsApproved        | BIT             | NOT NULL          | Moderation approval flag        |

## Key Relationships

### One-to-Many Relationships

- **User to Order**: A user can have multiple orders.
- **User to Address**: A user can have multiple addresses.
- **User to Review**: A user can write multiple reviews.
- **Category to Product**: A category can contain multiple products.
- **Product to Review**: A product can have multiple reviews.
- **Product to ProductOption**: A product can have multiple customization options.
- **ProductOption to ProductOptionValue**: An option can have multiple values.
- **Order to OrderItem**: An order contains multiple items.
- **ShoppingCart to ShoppingCartItem**: A shopping cart contains multiple items.

### Many-to-One Relationships

- **OrderItem to Product**: Each order item references a product.
- **ShoppingCartItem to Product**: Each cart item references a product.
- **Address to User**: Each address belongs to a user.
- **Review to User**: Each review is written by a user.
- **Review to Product**: Each review is for a product.
- **Product to Category**: Each product belongs to a category.

## Indexes

### Primary Key Indexes

- PK_Users (User.Id)
- PK_Addresses (Address.Id)
- PK_Categories (Category.Id)
- PK_Products (Product.Id)
- PK_ProductOptions (ProductOption.Id)
- PK_ProductOptionValues (ProductOptionValue.Id)
- PK_Orders (Order.Id)
- PK_OrderItems (OrderItem.Id)
- PK_ShoppingCarts (ShoppingCart.Id)
- PK_ShoppingCartItems (ShoppingCartItem.Id)
- PK_Reviews (Review.Id)

### Foreign Key Indexes

- FK_Addresses_Users (Address.UserId)
- FK_Products_Categories (Product.CategoryId)
- FK_ProductOptions_Products (ProductOption.ProductId)
- FK_ProductOptionValues_ProductOptions (ProductOptionValue.ProductOptionId)
- FK_Orders_Users (Order.UserId)
- FK_Orders_Addresses_Shipping (Order.ShippingAddressId)
- FK_Orders_Addresses_Billing (Order.BillingAddressId)
- FK_OrderItems_Orders (OrderItem.OrderId)
- FK_OrderItems_Products (OrderItem.ProductId)
- FK_ShoppingCarts_Users (ShoppingCart.UserId)
- FK_ShoppingCartItems_ShoppingCarts (ShoppingCartItem.ShoppingCartId)
- FK_ShoppingCartItems_Products (ShoppingCartItem.ProductId)
- FK_Reviews_Products (Review.ProductId)
- FK_Reviews_Users (Review.UserId)

### Additional Indexes

- IX_Users_Email (User.Email) - Unique
- IX_Users_UserName (User.UserName) - Unique
- IX_Orders_OrderNumber (Order.OrderNumber) - Unique
- IX_Orders_UserId_OrderDate (Order.UserId, Order.OrderDate)
- IX_Products_CategoryId (Product.CategoryId)
- IX_Products_Name (Product.Name)
- IX_ShoppingCart_UserId (ShoppingCart.UserId)
- IX_ShoppingCart_SessionId (ShoppingCart.SessionId)
- IX_Reviews_ProductId_Rating (Review.ProductId, Review.Rating)

## Database Migrations

The application uses Entity Framework Core with a code-first approach for database migrations. All migrations are stored in the `Migrations` folder of the project.

### Common Migration Commands

To create a new migration:
```
dotnet ef migrations add MigrationName
```

To update the database to the latest migration:
```
dotnet ef database update
```

To remove the last migration:
```
dotnet ef migrations remove
```

## Data Access Patterns

The application follows the Repository pattern for data access:

```csharp
// Example repository interface
public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAllAsync();
    Task<Product> GetByIdAsync(int id);
    Task<IEnumerable<Product>> GetByCategoryAsync(int categoryId);
    Task<Product> AddAsync(Product product);
    Task UpdateAsync(Product product);
    Task DeleteAsync(int id);
}
```

## Security Considerations

- Passwords are stored as hashes using ASP.NET Core Identity
- Connection strings use integrated security or environment variables for credentials
- Entity Framework parameterizes queries to prevent SQL injection
- Sensitive data (payment information) is not stored directly in the database 