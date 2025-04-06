# API Documentation

This document provides a reference for the Brew Haven Coffee Shop e-commerce platform's API endpoints.

## Authentication Endpoints

### User Registration

**POST /api/account/register**

Creates a new user account.

**Request Body:**
```json
{
  "email": "user@example.com",
  "password": "SecurePassword123!",
  "firstName": "John",
  "lastName": "Doe"
}
```

**Response (201 Created):**
```json
{
  "id": "user-guid",
  "email": "user@example.com",
  "firstName": "John",
  "lastName": "Doe"
}
```

### User Login

**POST /api/account/login**

Authenticates a user.

**Request Body:**
```json
{
  "email": "user@example.com",
  "password": "SecurePassword123!"
}
```

**Response (200 OK):**
```json
{
  "token": "JWT-TOKEN",
  "expiration": "2023-12-31T23:59:59Z",
  "user": {
    "id": "user-guid",
    "email": "user@example.com",
    "firstName": "John",
    "lastName": "Doe"
  }
}
```

## Product Endpoints

### Get All Products

**GET /api/products**

Retrieves a paginated list of products.

**Query Parameters:**
- `page` (default: 1) - Page number
- `pageSize` (default: 10) - Number of items per page
- `category` (optional) - Filter by category ID
- `searchTerm` (optional) - Search by name
- `sortBy` (optional) - Sort field (price, name, newest)
- `sortDirection` (optional) - asc or desc

**Response (200 OK):**
```json
{
  "items": [
    {
      "id": 1,
      "name": "Espresso",
      "description": "Strong coffee brewed by forcing hot water through finely-ground coffee beans",
      "price": 3.99,
      "imageUrl": "/images/products/espresso.jpg",
      "categoryId": 1,
      "categoryName": "Hot Coffee",
      "rating": 4.7,
      "inStock": true,
      "options": [
        {
          "id": 1,
          "name": "Size",
          "choices": ["Small", "Medium", "Large"]
        }
      ]
    }
  ],
  "totalCount": 25,
  "page": 1,
  "pageSize": 10,
  "totalPages": 3
}
```

### Get Product By ID

**GET /api/products/{id}**

Retrieves detailed information about a specific product.

**Response (200 OK):**
```json
{
  "id": 1,
  "name": "Espresso",
  "description": "Strong coffee brewed by forcing hot water through finely-ground coffee beans",
  "price": 3.99,
  "imageUrl": "/images/products/espresso.jpg",
  "categoryId": 1,
  "categoryName": "Hot Coffee",
  "rating": 4.7,
  "inStock": true,
  "options": [
    {
      "id": 1,
      "name": "Size",
      "choices": ["Small", "Medium", "Large"]
    },
    {
      "id": 2,
      "name": "Extra Shot",
      "choices": ["None", "Single", "Double"]
    }
  ],
  "reviews": [
    {
      "id": 1,
      "userId": "user-guid",
      "userName": "John D.",
      "rating": 5,
      "comment": "Excellent coffee!",
      "createdAt": "2023-10-15T14:30:00Z"
    }
  ],
  "relatedProducts": [2, 3, 4]
}
```

### Get Categories

**GET /api/categories**

Retrieves all product categories.

**Response (200 OK):**
```json
[
  {
    "id": 1,
    "name": "Hot Coffee",
    "description": "Freshly brewed hot coffee drinks",
    "imageUrl": "/images/categories/hot-coffee.jpg",
    "productCount": 8
  },
  {
    "id": 2,
    "name": "Cold Coffee",
    "description": "Refreshing cold coffee options",
    "imageUrl": "/images/categories/cold-coffee.jpg",
    "productCount": 6
  }
]
```

## Cart Endpoints

### Get Cart

**GET /api/cart**

Retrieves the current user's shopping cart.

**Response (200 OK):**
```json
{
  "id": "cart-guid",
  "items": [
    {
      "id": "cart-item-guid",
      "productId": 1,
      "productName": "Espresso",
      "quantity": 2,
      "unitPrice": 3.99,
      "totalPrice": 7.98,
      "imageUrl": "/images/products/espresso.jpg",
      "options": [
        {
          "name": "Size",
          "value": "Medium"
        }
      ]
    }
  ],
  "itemCount": 2,
  "subtotal": 7.98,
  "tax": 0.80,
  "total": 8.78
}
```

### Add Item to Cart

**POST /api/cart/items**

Adds a product to the shopping cart.

**Request Body:**
```json
{
  "productId": 1,
  "quantity": 2,
  "options": [
    {
      "optionId": 1,
      "value": "Medium"
    }
  ]
}
```

**Response (200 OK):**
```json
{
  "id": "cart-guid",
  "items": [
    {
      "id": "cart-item-guid",
      "productId": 1,
      "productName": "Espresso",
      "quantity": 2,
      "unitPrice": 3.99,
      "totalPrice": 7.98,
      "imageUrl": "/images/products/espresso.jpg",
      "options": [
        {
          "name": "Size",
          "value": "Medium"
        }
      ]
    }
  ],
  "itemCount": 2,
  "subtotal": 7.98,
  "tax": 0.80,
  "total": 8.78
}
```

### Update Cart Item

**PUT /api/cart/items/{id}**

Updates the quantity or options of a cart item.

**Request Body:**
```json
{
  "quantity": 3,
  "options": [
    {
      "optionId": 1,
      "value": "Large"
    }
  ]
}
```

**Response (200 OK):**
```json
{
  "id": "cart-item-guid",
  "productId": 1,
  "productName": "Espresso",
  "quantity": 3,
  "unitPrice": 3.99,
  "totalPrice": 11.97,
  "imageUrl": "/images/products/espresso.jpg",
  "options": [
    {
      "name": "Size",
      "value": "Large"
    }
  ]
}
```

### Remove Cart Item

**DELETE /api/cart/items/{id}**

Removes an item from the shopping cart.

**Response (204 No Content)**

## Order Endpoints

### Create Order

**POST /api/orders**

Creates a new order from the current cart.

**Request Body:**
```json
{
  "shippingAddress": {
    "firstName": "John",
    "lastName": "Doe",
    "streetAddress": "123 Coffee St",
    "city": "Seattle",
    "state": "WA",
    "zipCode": "98101",
    "country": "USA"
  },
  "paymentMethod": {
    "type": "credit_card",
    "cardNumber": "4111111111111111",
    "expirationMonth": 12,
    "expirationYear": 2025,
    "cvv": "123"
  }
}
```

**Response (201 Created):**
```json
{
  "id": "order-guid",
  "orderNumber": "BH-12345",
  "orderDate": "2023-11-01T15:30:00Z",
  "status": "pending",
  "items": [
    {
      "productId": 1,
      "productName": "Espresso",
      "quantity": 2,
      "unitPrice": 3.99,
      "totalPrice": 7.98,
      "options": [
        {
          "name": "Size",
          "value": "Medium"
        }
      ]
    }
  ],
  "subtotal": 7.98,
  "tax": 0.80,
  "total": 8.78,
  "paymentStatus": "pending",
  "paymentMethod": "credit_card",
  "shippingAddress": {
    "firstName": "John",
    "lastName": "Doe",
    "streetAddress": "123 Coffee St",
    "city": "Seattle",
    "state": "WA",
    "zipCode": "98101",
    "country": "USA"
  }
}
```

### Get Order History

**GET /api/orders**

Retrieves the order history for the current user.

**Query Parameters:**
- `page` (default: 1) - Page number
- `pageSize` (default: 10) - Number of items per page
- `status` (optional) - Filter by order status

**Response (200 OK):**
```json
{
  "items": [
    {
      "id": "order-guid",
      "orderNumber": "BH-12345",
      "orderDate": "2023-11-01T15:30:00Z",
      "status": "delivered",
      "total": 8.78,
      "itemCount": 2
    }
  ],
  "totalCount": 5,
  "page": 1,
  "pageSize": 10,
  "totalPages": 1
}
```

### Get Order Details

**GET /api/orders/{id}**

Retrieves detailed information about a specific order.

**Response (200 OK):**
```json
{
  "id": "order-guid",
  "orderNumber": "BH-12345",
  "orderDate": "2023-11-01T15:30:00Z",
  "status": "delivered",
  "items": [
    {
      "productId": 1,
      "productName": "Espresso",
      "quantity": 2,
      "unitPrice": 3.99,
      "totalPrice": 7.98,
      "imageUrl": "/images/products/espresso.jpg",
      "options": [
        {
          "name": "Size",
          "value": "Medium"
        }
      ]
    }
  ],
  "subtotal": 7.98,
  "tax": 0.80,
  "total": 8.78,
  "paymentStatus": "paid",
  "paymentMethod": "credit_card",
  "shippingAddress": {
    "firstName": "John",
    "lastName": "Doe",
    "streetAddress": "123 Coffee St",
    "city": "Seattle",
    "state": "WA",
    "zipCode": "98101",
    "country": "USA"
  },
  "trackingInfo": {
    "carrier": "Local Delivery",
    "trackingNumber": "LD12345678",
    "estimatedDelivery": "2023-11-02T14:00:00Z",
    "status": "delivered",
    "deliveredAt": "2023-11-02T13:45:00Z"
  }
}
```

## Review Endpoints

### Submit Product Review

**POST /api/products/{id}/reviews**

Submits a review for a product.

**Request Body:**
```json
{
  "rating": 5,
  "comment": "Excellent coffee! Rich flavor and perfect temperature."
}
```

**Response (201 Created):**
```json
{
  "id": "review-guid",
  "productId": 1,
  "userId": "user-guid",
  "userName": "John D.",
  "rating": 5,
  "comment": "Excellent coffee! Rich flavor and perfect temperature.",
  "createdAt": "2023-11-05T10:15:00Z"
}
```

### Get Product Reviews

**GET /api/products/{id}/reviews**

Retrieves reviews for a specific product.

**Query Parameters:**
- `page` (default: 1) - Page number
- `pageSize` (default: 10) - Number of items per page
- `sortBy` (optional) - Sort field (date, rating)
- `sortDirection` (optional) - asc or desc

**Response (200 OK):**
```json
{
  "items": [
    {
      "id": "review-guid",
      "userId": "user-guid",
      "userName": "John D.",
      "rating": 5,
      "comment": "Excellent coffee! Rich flavor and perfect temperature.",
      "createdAt": "2023-11-05T10:15:00Z"
    }
  ],
  "totalCount": 15,
  "page": 1,
  "pageSize": 10,
  "totalPages": 2,
  "averageRating": 4.7
}
```

## User Profile Endpoints

### Get User Profile

**GET /api/account/profile**

Retrieves the current user's profile information.

**Response (200 OK):**
```json
{
  "id": "user-guid",
  "email": "user@example.com",
  "firstName": "John",
  "lastName": "Doe",
  "phoneNumber": "555-123-4567",
  "addresses": [
    {
      "id": "address-guid",
      "type": "shipping",
      "isDefault": true,
      "firstName": "John",
      "lastName": "Doe",
      "streetAddress": "123 Coffee St",
      "city": "Seattle",
      "state": "WA",
      "zipCode": "98101",
      "country": "USA"
    }
  ],
  "preferences": {
    "marketingEmails": true,
    "orderNotifications": true
  }
}
```

### Update User Profile

**PUT /api/account/profile**

Updates the current user's profile information.

**Request Body:**
```json
{
  "firstName": "John",
  "lastName": "Doe",
  "phoneNumber": "555-123-4567",
  "preferences": {
    "marketingEmails": false,
    "orderNotifications": true
  }
}
```

**Response (200 OK):**
```json
{
  "id": "user-guid",
  "email": "user@example.com",
  "firstName": "John",
  "lastName": "Doe",
  "phoneNumber": "555-123-4567",
  "preferences": {
    "marketingEmails": false,
    "orderNotifications": true
  }
}
```

### Add Address

**POST /api/account/addresses**

Adds a new address to the user profile.

**Request Body:**
```json
{
  "type": "shipping",
  "isDefault": true,
  "firstName": "John",
  "lastName": "Doe",
  "streetAddress": "123 Coffee St",
  "city": "Seattle",
  "state": "WA",
  "zipCode": "98101",
  "country": "USA"
}
```

**Response (201 Created):**
```json
{
  "id": "address-guid",
  "type": "shipping",
  "isDefault": true,
  "firstName": "John",
  "lastName": "Doe",
  "streetAddress": "123 Coffee St",
  "city": "Seattle",
  "state": "WA",
  "zipCode": "98101",
  "country": "USA"
}
```

## Admin API Endpoints

### Admin: Create Product

**POST /api/admin/products**

Creates a new product (admin only).

**Request Body:**
```json
{
  "name": "New Espresso",
  "description": "Our newest espresso blend",
  "price": 4.99,
  "categoryId": 1,
  "inStock": true,
  "options": [
    {
      "name": "Size",
      "choices": ["Small", "Medium", "Large"]
    }
  ]
}
```

**Response (201 Created):**
```json
{
  "id": 10,
  "name": "New Espresso",
  "description": "Our newest espresso blend",
  "price": 4.99,
  "imageUrl": null,
  "categoryId": 1,
  "categoryName": "Hot Coffee",
  "inStock": true,
  "options": [
    {
      "id": 20,
      "name": "Size",
      "choices": ["Small", "Medium", "Large"]
    }
  ]
}
```

### Admin: Update Order Status

**PUT /api/admin/orders/{id}/status**

Updates the status of an order (admin only).

**Request Body:**
```json
{
  "status": "shipped",
  "trackingInfo": {
    "carrier": "Local Delivery",
    "trackingNumber": "LD12345678",
    "estimatedDelivery": "2023-11-02T14:00:00Z"
  }
}
```

**Response (200 OK):**
```json
{
  "id": "order-guid",
  "orderNumber": "BH-12345",
  "status": "shipped",
  "trackingInfo": {
    "carrier": "Local Delivery",
    "trackingNumber": "LD12345678",
    "estimatedDelivery": "2023-11-02T14:00:00Z",
    "status": "in_transit"
  }
}
```

## Error Responses

All API endpoints return standard HTTP status codes and a JSON response body for errors:

**400 Bad Request:**
```json
{
  "status": 400,
  "title": "Bad Request",
  "errors": {
    "email": ["Email is required"],
    "password": ["Password must be at least 8 characters"]
  }
}
```

**401 Unauthorized:**
```json
{
  "status": 401,
  "title": "Unauthorized",
  "detail": "Authentication credentials are missing or invalid"
}
```

**403 Forbidden:**
```json
{
  "status": 403,
  "title": "Forbidden",
  "detail": "You do not have permission to access this resource"
}
```

**404 Not Found:**
```json
{
  "status": 404,
  "title": "Not Found",
  "detail": "The requested resource was not found"
}
```

**500 Internal Server Error:**
```json
{
  "status": 500,
  "title": "Internal Server Error",
  "detail": "An unexpected error occurred while processing your request"
}
```

## Versioning

API versioning is handled via the URL path (e.g., `/api/v1/products`) or via an `Accept` header:

```
Accept: application/json; version=1.0
```

## Rate Limiting

API endpoints are rate-limited to prevent abuse. Rate limit information is included in the response headers:

```
X-RateLimit-Limit: 100
X-RateLimit-Remaining: 99
X-RateLimit-Reset: 1635774000
```

## Authentication

All authenticated endpoints require a valid JWT token provided in the Authorization header:

```
Authorization: Bearer <token>
```

Tokens can be obtained via the `/api/account/login` endpoint and expire after 24 hours. 