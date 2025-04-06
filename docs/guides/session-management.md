# Session Management Guide

This document provides details on how session management is implemented in the Reina.MacCredy E-Commerce Platform, with a focus on shopping cart functionality and user state.

## Overview

The application uses ASP.NET Core's session middleware with custom extensions to store complex objects like shopping carts. Session management is critical for maintaining user state, especially for unauthenticated users.

## Session Configuration

Session is configured in `Program.cs`:

```csharp
// Add session services
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SameSite = SameSiteMode.Lax;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});

// Add session middleware to the pipeline
app.UseSession();
```

## Session Extension Methods

Custom extension methods in `Extensions/SessionExtensions.cs` provide type-safe access to session data:

```csharp
public static class SessionExtensions
{
    // Retrieve an object from session
    public static T GetObjectFromJson<T>(this ISession session, string key)
    {
        try
        {
            var value = session.GetString(key);
            return value == null ? default : JsonConvert.DeserializeObject<T>(value);
        }
        catch (Exception ex)
        {
            // Log the exception
            return default;
        }
    }

    // Store an object in session
    public static void SetObjectAsJson(this ISession session, string key, object value)
    {
        try
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }
        catch (Exception ex)
        {
            // Log the exception
        }
    }

    // Remove an object from session
    public static void RemoveObject(this ISession session, string key)
    {
        session.Remove(key);
    }
}
```

## Shopping Cart Implementation

The shopping cart is a key component that relies on session management:

### Cart Model

```csharp
public class ShoppingCart
{
    public List<CartItem> Items { get; set; } = new List<CartItem>();
    
    // Required property for proper JSON serialization
    public decimal TotalPrice
    {
        get { return Items.Sum(item => item.Quantity * item.UnitPrice); }
        set { /* Required for deserialization */ }
    }
}

public class CartItem
{
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
    public string Size { get; set; }
    public string Customizations { get; set; }
    public string ImageUrl { get; set; }
}
```

### Cart Controller

The `ShoppingCartController` handles cart operations using session:

```csharp
public class ShoppingCartController : Controller
{
    private readonly IProductRepository _productRepository;
    private const string CartSessionKey = "ShoppingCart";

    public ShoppingCartController(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>(CartSessionKey) ?? new ShoppingCart();
        return View(cart);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddToCart(int productId, int quantity, string size, string customizations)
    {
        try
        {
            // Get or create the cart
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>(CartSessionKey) ?? new ShoppingCart();
            
            // Get the product
            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null)
            {
                return NotFound("Product not found");
            }

            // Check if product already exists in cart with same options
            var existingItem = cart.Items.FirstOrDefault(i => 
                i.ProductId == productId && 
                i.Size == size && 
                i.Customizations == customizations);

            if (existingItem != null)
            {
                // Update quantity if item exists
                existingItem.Quantity += quantity;
            }
            else
            {
                // Add new item to cart
                cart.Items.Add(new CartItem
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    UnitPrice = product.Price,
                    Quantity = quantity,
                    Size = size,
                    Customizations = customizations,
                    ImageUrl = product.ImageUrl
                });
            }

            // Save cart back to session
            HttpContext.Session.SetObjectAsJson(CartSessionKey, cart);

            // Handle AJAX requests
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new 
                { 
                    success = true, 
                    itemCount = cart.Items.Sum(i => i.Quantity),
                    totalPrice = cart.TotalPrice.ToString("N0") + " VND"
                });
            }

            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            // Log the exception
            
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = false, message = "Failed to add item to cart" });
            }
            
            return RedirectToAction("Index");
        }
    }

    // Other cart operations...
}
```

## Common Session Management Scenarios

### 1. Cart Synchronization for Authenticated Users

When a user logs in, the application syncs the session cart with the user's database cart:

```csharp
// In AccountController after successful login
private async Task SyncShoppingCartAsync(string userId)
{
    // Get session cart
    var sessionCart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("ShoppingCart");
    if (sessionCart == null || !sessionCart.Items.Any())
    {
        return;
    }
    
    // Get user's stored cart from database
    var userCart = await _cartRepository.GetByUserIdAsync(userId);
    
    if (userCart == null)
    {
        // Create new cart for user
        userCart = new UserCart { UserId = userId, Items = new List<UserCartItem>() };
    }
    
    // Merge carts
    foreach (var sessionItem in sessionCart.Items)
    {
        var existingItem = userCart.Items.FirstOrDefault(i => 
            i.ProductId == sessionItem.ProductId && 
            i.Size == sessionItem.Size && 
            i.Customizations == sessionItem.Customizations);
            
        if (existingItem != null)
        {
            existingItem.Quantity += sessionItem.Quantity;
        }
        else
        {
            userCart.Items.Add(new UserCartItem
            {
                ProductId = sessionItem.ProductId,
                Quantity = sessionItem.Quantity,
                Size = sessionItem.Size,
                Customizations = sessionItem.Customizations
            });
        }
    }
    
    // Save merged cart to database
    await _cartRepository.SaveAsync(userCart);
    
    // Clear session cart (now using database cart)
    HttpContext.Session.Remove("ShoppingCart");
}
```

### 2. AJAX Cart Updates

The application uses AJAX to update the cart without page reloads:

```javascript
// In cart.js
function addToCart(productId, quantity, size, customizations) {
    $.ajax({
        url: '/ShoppingCart/AddToCart',
        type: 'POST',
        data: {
            productId: productId,
            quantity: quantity,
            size: size,
            customizations: customizations,
            __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val()
        },
        headers: {
            'X-Requested-With': 'XMLHttpRequest'
        },
        success: function(response) {
            if (response.success) {
                // Update cart count in navbar
                $('#cart-count').text(response.itemCount);
                
                // Show success toast
                showToast('Success', 'Item added to cart!', 'success');
            } else {
                showToast('Error', response.message, 'error');
            }
        },
        error: function() {
            showToast('Error', 'Failed to add item to cart', 'error');
        }
    });
}
```

## Session Security

The application implements several security measures for session management:

1. **HTTPS**: Sessions are transmitted over secure connections
2. **HttpOnly Cookies**: Prevents JavaScript access to session cookies
3. **Secure Cookies**: Cookies only sent over HTTPS
4. **SameSite Policy**: Restricts cookie use to same site
5. **Anti-Forgery Tokens**: CSRF protection for form submissions
6. **Essential Cookies**: Marked as essential for GDPR compliance
7. **Timeout Management**: 30-minute idle timeout for sessions

## Common Issues and Solutions

### 1. Session Serialization Errors

**Issue**: `Invalid Operation Exception: Error during serialization or deserialization`

**Solution**: Ensure all serialized objects have proper setters and default constructors:

```csharp
// Correct:
public decimal TotalPrice
{
    get { return Items.Sum(item => item.Quantity * item.UnitPrice); }
    set { /* Empty setter for serialization */ }
}

// Not correct:
public decimal TotalPrice => Items.Sum(item => item.Quantity * item.UnitPrice);
```

### 2. Session State Loss

**Issue**: Session state disappears unexpectedly

**Solutions**:
- Check idle timeout configuration (default is 30 minutes)
- Ensure distributed cache is properly configured
- Check for server restarts or web garden scenarios
- Verify session cookie settings

### 3. Cart Persistence for Guest Users

**Issue**: Guest users losing cart on browser close

**Solution**: Implement a cart that persists using cookies or local storage as a fallback:

```javascript
// Store cart locally when session might expire
function backupCartLocally(cart) {
    localStorage.setItem('cartBackup', JSON.stringify(cart));
}

// Restore cart from local backup if session is empty
function restoreCartFromBackup() {
    var backupCart = localStorage.getItem('cartBackup');
    if (backupCart) {
        $.ajax({
            url: '/ShoppingCart/RestoreCart',
            type: 'POST',
            data: {
                cartData: backupCart,
                __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val()
            },
            success: function(response) {
                if (response.success) {
                    window.location.reload();
                }
            }
        });
    }
}
```

## Best Practices

1. **Minimal Session Data**: Store only essential information in session
2. **Error Handling**: Implement robust error handling for serialization
3. **Timeout Management**: Configure appropriate timeout periods
4. **Security Headers**: Set proper security headers for session cookies
5. **Anti-Forgery Tokens**: Always use for state-changing operations
6. **Fallback Mechanisms**: Implement fallbacks for session failures
7. **Performance Monitoring**: Track session size and performance impact 