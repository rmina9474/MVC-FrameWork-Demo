using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Reina.MacCredy.Extensions;
using Reina.MacCredy.Models;
using Reina.MacCredy.Repositories;
using Reina.MacCredy.Services;

namespace Reina.MacCredy.Controllers
{
    // Remove class-level Authorize attribute to allow some methods to be accessed by guests
    public class ShoppingCartController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly PaymentService _paymentService;
        
        public ShoppingCartController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IProductRepository productRepository, PaymentService paymentService)
        {
            _productRepository = productRepository;
            _context = context;
            _userManager = userManager;
            _paymentService = paymentService;
        }
        
        // Remove Authorize to allow guests to access checkout page
        public async Task<IActionResult> Checkout()
        {
            // Check if cart exists and has items
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");
            if (cart == null || !cart.Items.Any())
            {
                return RedirectToAction("Index", "Home");
            }
            
            // For logged in users, pre-fill information
            if (User?.Identity != null && User.Identity.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    return View(new Order
                    {
                        UserId = user.Id,
                        ApplicationUser = user,
                        ShippingAddress = string.Empty,
                        OrderDetails = new List<OrderDetail>()
                    });
                }
            }
            
            // For guest users, show a different checkout view
            return View("GuestCheckout", new Order
            {
                OrderDetails = new List<OrderDetail>()
            });
        }
        
        // Remove Authorize to allow guests to submit orders
        [HttpPost]
        public async Task<IActionResult> Checkout(Order order)
        {
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");
            
            if (cart == null || !cart.Items.Any())
            {
                ModelState.AddModelError("", "Your cart is empty!");
                return View(order);
            }
            
            // Add current cart items to order
            foreach (var item in cart.Items)
            {
                var orderDetail = new OrderDetail
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Price,
                    SelectedOptions = item.SelectedOptions
                };
                order.OrderDetails.Add(orderDetail);
            }
            
            order.TotalPrice = cart.GetTotal();
            order.OrderDate = DateTime.Now;
            
            // Set user ID if authenticated
            if (User?.Identity != null && User.Identity.IsAuthenticated)
            {
                order.UserId = await _userManager.GetUserIdAsync(await _userManager.GetUserAsync(User));
                order.IsGuestOrder = false;
            }
            else
            {
                order.IsGuestOrder = true;
            }
            
            // Validate notes length
            if (order.Notes?.Length > 500)
            {
                ModelState.AddModelError("Notes", "Notes cannot exceed 500 characters");
                return View(order);
            }
            
            // Set default notes if empty
            if (string.IsNullOrWhiteSpace(order.Notes))
            {
                order.Notes = "No additional notes.";
            }

            // Ensure PaymentMethod has a valid value (default to Cash on Delivery if not provided)
            if (order.PaymentMethod <= 0)
            {
                order.PaymentMethod = SD.Payment_CashOnDelivery; // Use SD constant
            }
            
            // Set default payment status
            order.PaymentStatus = SD.PaymentStatus_Pending; // Use SD constant

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            
            // Process payment if not Cash on Delivery
            if (order.PaymentMethod != SD.Payment_CashOnDelivery)
            {
                var returnUrl = Url.Action("OrderCompleted", "ShoppingCart", new { id = order.Id }, Request.Scheme);
                var paymentResult = await _paymentService.ProcessPaymentAsync(order, returnUrl);

                if (paymentResult.Success)
                {
                    // Update order with payment success
                    order.PaymentStatus = SD.PaymentStatus_Approved;
                    order.PaymentResponse = paymentResult.Message;
                    await _context.SaveChangesAsync();
                    
                    // Clear the cart
                    HttpContext.Session.Remove("Cart");
                    
                    // Redirect based on payment result
                    if (!string.IsNullOrEmpty(paymentResult.RedirectUrl))
                    {
                        // Check if this is an external redirect or an internal route
                        if (paymentResult.RedirectUrl.StartsWith("http") && 
                            !paymentResult.RedirectUrl.Contains(Request.Host.ToString()))
                        {
                            // External payment gateway redirect
                            return Redirect(paymentResult.RedirectUrl);
                        }
                        else
                        {
                            // Internal route - redirect to route within our application
                            return Redirect(paymentResult.RedirectUrl);
                        }
                    }
                    else
                    {
                        // Direct completion (no redirect needed)
                        TempData["OrderId"] = order.Id.ToString();
                        TempData["PaymentMessage"] = paymentResult.Message;
                        return RedirectToAction("OrderCompleted", new { id = order.Id });
                    }
                }
                else
                {
                    // Payment processing failed
                    ModelState.AddModelError("", paymentResult.Message);
                    return View(order);
                }
            }
            else
            {
                // Cash on delivery - mark as pending
                order.PaymentStatus = SD.PaymentStatus_Pending;
                await _context.SaveChangesAsync();
            }
            
            // Clear the cart
            HttpContext.Session.Remove("Cart");
            
            // Use TempData which is more reliable across redirects than ViewBag
            TempData["OrderId"] = order.Id.ToString();
            return RedirectToAction("OrderCompleted", new { id = order.Id });
        }

        public IActionResult OrderCompleted(int id)
        {
            // Retrieve order details if needed
            var order = _context.Orders.FirstOrDefault(o => o.Id == id);
            
            if (order == null)
            {
                // Fallback to TempData if order not found
                ViewBag.OrderId = TempData["OrderId"]?.ToString() ?? "Unknown";
                ViewBag.PaymentMessage = TempData["PaymentMessage"]?.ToString() ?? "Order processed successfully";
                // Set default payment status to pending (integer value)
                ViewBag.PaymentStatus = SD.PaymentStatus_Pending;
                ViewBag.PaymentMethod = SD.Payment_CashOnDelivery;
            }
            else
            {
                ViewBag.OrderId = order.Id.ToString();
                ViewBag.PaymentStatus = order.PaymentStatus;
                ViewBag.PaymentMethod = GetPaymentMethodName(order.PaymentMethod);
                ViewBag.PaymentMessage = order.PaymentResponse ?? "Order processed successfully";
            }
            
            return View("OrderCompleted");
        }

        // Helper method to get payment method name
        private string GetPaymentMethodName(int paymentMethod)
        {
            return paymentMethod switch
            {
                SD.Payment_CashOnDelivery => "Cash on Delivery",
                SD.Payment_CreditCard => "Credit Card",
                SD.Payment_BankTransfer => "Bank Transfer",
                SD.Payment_MoMo => "MoMo",
                SD.Payment_VNPay => "VNPay",
                _ => "Unknown"
            };
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, int quantity = 1, decimal totalPrice = 0, string selectedOptions = "")
        {
            try
            {
                var product = await _productRepository.GetByIdAsync(productId);
                if (product == null)
                {
                    if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    {
                        return Json(new { success = false, message = "Product not found" });
                    }
                    return NotFound();
                }

                // Get cart from session or create a new one
                var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");
                if (cart == null)
                {
                    cart = new ShoppingCart();
                }
                
                // Use custom price if provided (for products with options)
                decimal actualPrice = totalPrice > 0 ? totalPrice : product.Price;
                
                // Create a unique identifier for this product+options combination
                string cartItemKey = productId.ToString();
                if (!string.IsNullOrEmpty(selectedOptions))
                {
                    cartItemKey += "_" + selectedOptions.GetHashCode();
                }
                
                var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == productId && 
                                                        (i.SelectedOptions == selectedOptions || 
                                                        (string.IsNullOrEmpty(i.SelectedOptions) && string.IsNullOrEmpty(selectedOptions))));
                
                if (existingItem != null)
                {
                    existingItem.Quantity += quantity;
                }
                else
                {
                    cart.Items.Add(new CartItem
                    {
                        ProductId = product.Id,
                        Name = product.Name,
                        Price = actualPrice,
                        Quantity = quantity,
                        SelectedOptions = selectedOptions
                    });
                }

                // Save updated cart back to session
                HttpContext.Session.SetObjectAsJson("Cart", cart);
                
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { 
                        success = true, 
                        message = "Product added to order successfully",
                        quantity = quantity,
                        cartCount = cart.Items.Sum(i => i.Quantity),
                        cartTotal = cart.TotalPrice
                    });
                }
                
                // Check if user is authenticated before proceeding to cart
                if (User?.Identity == null || !User.Identity.IsAuthenticated)
                {
                    // Store return URL for after login
                    return RedirectToAction("LoginPrompt", new { returnUrl = Url.Action("Index", "ShoppingCart") });
                }
                
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = false, message = $"Error adding to cart: {ex.Message}", error = ex.Message });
                }
                throw;
            }
        }

        [HttpGet("ShoppingCart/RemoveFromCart")]
        public IActionResult RemoveFromCart(int productId)
        {
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");
            if (cart != null)
            {
                cart.RemoveItem(productId);
                HttpContext.Session.SetObjectAsJson("Cart", cart);
            }

            return RedirectToAction("Index", "ShoppingCart");
        }

        // Display the shopping cart contents
        public IActionResult Index()
        {
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart") ?? new ShoppingCart();
            return View(cart);
        }

        [HttpGet]
        public IActionResult GetCartCount()
        {
            try
            {
                var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");
                var cartCount = cart?.Items.Sum(item => item.Quantity) ?? 0;
                return Json(cartCount);
            }
            catch (Exception ex)
            {
                return Json(0); // Return 0 as fallback on error
            }
        }

        [HttpPost]
        public IActionResult UpdateQuantity(int productId, int quantity, string selectedOptions = "")
        {
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart") ?? new ShoppingCart();
            
            var item = cart.Items.FirstOrDefault(i => i.ProductId == productId && 
                                                    (i.SelectedOptions == selectedOptions || 
                                                     (string.IsNullOrEmpty(i.SelectedOptions) && string.IsNullOrEmpty(selectedOptions))));
            
            if (item != null)
            {
                item.Quantity = Math.Max(1, Math.Min(10, quantity)); // Ensure quantity is between 1 and 10
                HttpContext.Session.SetObjectAsJson("Cart", cart);
                return Json(new { success = true });
            }
            
            return Json(new { success = false });
        }

        // Add new method to prompt user to login or continue as guest
        public IActionResult LoginPrompt(string returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl ?? Url.Action("Checkout", "ShoppingCart");
            return View();
        }

        // Add method to transfer guest cart to user cart after login
        [Authorize]
        public async Task<IActionResult> TransferGuestCart()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }

            // Get the guest cart from session
            var guestCart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart") ?? new ShoppingCart();
            
            // If we had a user-specific cart in database, we could merge them here
            // For now, we just keep the session cart

            return RedirectToAction("Index");
        }
    }
}
