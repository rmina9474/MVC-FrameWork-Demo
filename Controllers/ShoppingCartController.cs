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
        private readonly IPaymentService _paymentService;

        public ShoppingCartController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            IProductRepository productRepository,
            IPaymentService paymentService)
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(Order order)
        {
            Console.WriteLine("======= CHECKOUT POST METHOD CALLED =======");
            Console.WriteLine($"Form submission received by controller!");

            try
            {
                Console.WriteLine($"Order data: ShippingAddress='{order.ShippingAddress}', PaymentMethod={order.PaymentMethod}");

                // Log form data for debugging
                Console.WriteLine("Form values:");
                foreach (var key in Request.Form.Keys)
                {
                    Console.WriteLine($"- {key}: {Request.Form[key]}");
                }

                // Check for model validity
                if (!ModelState.IsValid)
                {
                    Console.WriteLine("Model state is invalid:");
                    foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                    {
                        Console.WriteLine($"- {error.ErrorMessage}");
                    }

                    var viewName = User?.Identity?.IsAuthenticated == true ? "Checkout" : "GuestCheckout";
                    return View(viewName, order);
                }

                var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");
                if (cart == null || !cart.Items.Any())
                {
                    Console.WriteLine("Cart is empty, redirecting to Index");
                    return RedirectToAction("Index");
                }

                Console.WriteLine($"Cart items count: {cart.Items.Count}");

                // Set up the order data
                if (User?.Identity?.IsAuthenticated == true)
                {
                    var user = await _userManager.GetUserAsync(User);
                    if (user != null)
                    {
                        order.UserId = user.Id;
                        Console.WriteLine($"User authenticated: {user.Id}");
                    }
                }
                else
                {
                    order.IsGuestOrder = true;
                    Console.WriteLine("Guest order");

                    // For guest users, ensure we have minimum required information
                    if (string.IsNullOrEmpty(order.ShippingAddress) || string.IsNullOrEmpty(order.Email))
                    {
                        Console.WriteLine("Missing required guest information");
                        ModelState.AddModelError("", "Please provide shipping address and email");
                        return View("GuestCheckout", order);
                    }
                }

                order.OrderDate = DateTime.UtcNow;
                order.TotalPrice = cart.Items.Sum(i => i.Price * i.Quantity);
                var orderDetails = new List<OrderDetail>();

                foreach (var item in cart.Items)
                {
                    var product = await _productRepository.GetByIdAsync(item.ProductId);
                    orderDetails.Add(new OrderDetail
                    {
                        ProductId = item.ProductId,
                        Product = product,
                        Quantity = item.Quantity,
                        Price = item.Price,
                        Order = order
                    });
                }
                order.OrderDetails = orderDetails;

                // Set a default value for Notes if it is null or empty
                if (string.IsNullOrEmpty(order.Notes))
                {
                    order.Notes = "No additional notes.";
                }

                // Save the order to the database
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                // Check if payment methods are supported in the database
                // Process payment based on selected payment method
                if (order.PaymentMethod != PaymentMethod.CashOnDelivery)
                {
                    // Only attempt to use payment gateways if we have a valid order ID
                    if (order.Id > 0)
                    {
                        try
                        {
                            switch (order.PaymentMethod)
                            {
                                case PaymentMethod.MoMo:
                                    var momoResult = await _paymentService.ProcessMoMoPaymentAsync(order);
                                    if (momoResult.success)
                                    {
                                        return Redirect(momoResult.redirectUrl);
                                    }
                                    else
                                    {
                                        TempData["ErrorMessage"] = momoResult.message;
                                    }
                                    break;

                                case PaymentMethod.VNPay:
                                    var vnpayResult = await _paymentService.ProcessVNPayPaymentAsync(order);
                                    if (vnpayResult.success)
                                    {
                                        return Redirect(vnpayResult.redirectUrl);
                                    }
                                    else
                                    {
                                        TempData["ErrorMessage"] = vnpayResult.message;
                                    }
                                    break;
                            }
                        }
                        catch (Exception ex)
                        {
                            // Log the payment processing error
                            TempData["ErrorMessage"] = $"Error processing payment: {ex.Message}";
                        }
                    }
                }

                // Clear the cart and go to the order completed page
                HttpContext.Session.Remove("Cart");
                Console.WriteLine("Order completed successfully, ID: " + order.Id);
                return RedirectToAction("OrderCompleted", "Order", new { id = order.Id });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"INNER ERROR: {ex.InnerException.Message}");
                }

                ModelState.AddModelError("", "An error occurred while processing your order. Please try again.");
                return View(User?.Identity?.IsAuthenticated == true ? "Checkout" : "GuestCheckout", order);
            }
        }

        [HttpGet]
        [Route("/ShoppingCart/MoMoCallback")]
        public async Task<IActionResult> MoMoCallback()
        {
            // Extract query parameters
            var callbackParams = Request.Query.ToDictionary(x => x.Key, x => x.Value.ToString());

            // Check if this is a cancellation
            if (callbackParams.TryGetValue("resultCode", out string? resultCode))
            {
                // ResultCode values: 0 = success, 1006 = user cancelled, others = various errors
                if (resultCode == "1006" || resultCode != "0")
                {
                    // User cancelled the payment or there was an error
                    TempData["InfoMessage"] = "The payment was cancelled or unsuccessful.";
                    return RedirectToAction("Index", "ShoppingCart");
                }
            }

            // Verify the callback
            var verificationResult = await _paymentService.VerifyPaymentCallbackAsync("momo", callbackParams);

            if (verificationResult.success)
            {
                // Extract order ID from callback parameters
                if (callbackParams.TryGetValue("orderId", out string? orderId) && !string.IsNullOrEmpty(orderId))
                {
                    // Parse order ID (format: ORDER_{id}_{timestamp})
                    var orderIdParts = orderId.Split('_');
                    if (orderIdParts.Length >= 2 && int.TryParse(orderIdParts[1], out int id))
                    {
                        try
                        {
                            // Update order payment status
                            var order = await _context.Orders.FindAsync(id);
                            if (order != null)
                            {
                                try
                                {
                                    // Try to update payment fields if they exist
                                    order.PaymentStatus = PaymentStatus.Completed;
                                    order.TransactionId = callbackParams.TryGetValue("transId", out string? transId) ? transId : null;
                                    order.PaymentResponse = System.Text.Json.JsonSerializer.Serialize(callbackParams);
                                    await _context.SaveChangesAsync();
                                }
                                catch (Exception ex)
                                {
                                    // If payment fields don't exist in the database, just log it
                                    Console.WriteLine($"Could not update payment details: {ex.Message}");

                                    // But still consider the payment successful
                                    TempData["SuccessMessage"] = "Your payment was successful, but some payment details could not be saved.";
                                }

                                // Clear cart
                                HttpContext.Session.Remove("Cart");

                                // Redirect to order completed page
                                return RedirectToAction("OrderCompleted", "Order", new { id = order.Id });
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error processing MoMo callback: {ex.Message}");
                            TempData["ErrorMessage"] = "Error processing payment callback. Please contact customer support.";
                        }
                    }
                }
            }

            // Handle verification failure
            TempData["ErrorMessage"] = verificationResult.message;
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Route("/ShoppingCart/VNPayCallback")]
        public async Task<IActionResult> VNPayCallback()
        {
            // Extract query parameters
            var callbackParams = Request.Query.ToDictionary(x => x.Key, x => x.Value.ToString());

            // Check if this is a cancellation or error
            if (callbackParams.TryGetValue("vnp_ResponseCode", out string? responseCode))
            {
                // ResponseCode: 00 = success, 24 = user cancelled, others = various errors
                if (responseCode == "24" || responseCode != "00")
                {
                    // User cancelled the payment or there was an error
                    TempData["InfoMessage"] = "The payment was cancelled or unsuccessful.";
                    return RedirectToAction("Index", "ShoppingCart");
                }
            }

            // Verify the callback
            var verificationResult = await _paymentService.VerifyPaymentCallbackAsync("vnpay", callbackParams);

            if (verificationResult.success)
            {
                // Extract order ID from callback parameters
                if (callbackParams.TryGetValue("vnp_TxnRef", out string? txnRef) && !string.IsNullOrEmpty(txnRef))
                {
                    // Parse order ID (format: {timestamp}_{id})
                    var txnRefParts = txnRef.Split('_');
                    if (txnRefParts.Length >= 2 && int.TryParse(txnRefParts[1], out int id))
                    {
                        try
                        {
                            // Update order payment status
                            var order = await _context.Orders.FindAsync(id);
                            if (order != null)
                            {
                                try
                                {
                                    // Try to update payment fields if they exist
                                    order.PaymentStatus = PaymentStatus.Completed;
                                    order.TransactionId = callbackParams.TryGetValue("vnp_TransactionNo", out string? transNo) ? transNo : null;
                                    order.PaymentResponse = System.Text.Json.JsonSerializer.Serialize(callbackParams);
                                    await _context.SaveChangesAsync();
                                }
                                catch (Exception ex)
                                {
                                    // If payment fields don't exist in the database, just log it
                                    Console.WriteLine($"Could not update payment details: {ex.Message}");

                                    // But still consider the payment successful
                                    TempData["SuccessMessage"] = "Your payment was successful, but some payment details could not be saved.";
                                }

                                // Clear cart
                                HttpContext.Session.Remove("Cart");

                                // Redirect to order completed page
                                return RedirectToAction("OrderCompleted", "Order", new { id = order.Id });
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error processing VNPay callback: {ex.Message}");
                            TempData["ErrorMessage"] = "Error processing payment callback. Please contact customer support.";
                        }
                    }
                }
            }

            // Handle verification failure
            TempData["ErrorMessage"] = verificationResult.message;
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, int quantity = 1, decimal totalPrice = 0, string selectedOptions = "")
        {
            try
            {
                var product = await _productRepository.GetByIdAsync(productId);
                if (product == null)
                {
                    return Json(new { success = false, message = "Product not found" });
                }

                // Get or create the shopping cart
                var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart") ?? new ShoppingCart();

                // Check if the product is already in the cart with the same options
                var existingItem = cart.Items.FirstOrDefault(item =>
                    item.ProductId == productId && item.SelectedOptions == selectedOptions);

                if (existingItem != null)
                {
                    // Update quantity if product already exists
                    existingItem.Quantity += quantity;
                }
                else
                {
                    // Calculate price (including options)
                    var price = totalPrice > 0 ? totalPrice : product.Price;

                    // Add new item to cart
                    cart.Items.Add(new CartItem
                    {
                        ProductId = productId,
                        Name = product.Name,
                        Price = price,
                        Quantity = quantity,
                        ImageUrl = product.ImageUrl,
                        SelectedOptions = selectedOptions
                    });
                }

                // Save cart back to session
                HttpContext.Session.SetObjectAsJson("Cart", cart);

                return Json(new { success = true, message = "Product added to cart", cartCount = cart.Items.Sum(i => i.Quantity) });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error adding product to cart: {ex.Message}" });
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

        // Thêm phương thức Index để hiển thị giỏ hàng (nếu chưa có)
        public IActionResult Index()
        {
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart") ?? new ShoppingCart();

            Console.WriteLine("Cart Items Count: " + cart.Items.Count);
            foreach (var item in cart.Items)
            {
                Console.WriteLine($"Item: {item.Name}, Price: {item.Price}, Quantity: {item.Quantity}");
            }

            return View(cart);
        }

        [HttpGet]
        public IActionResult GetCartCount()
        {
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");
            var cartCount = cart?.Items.Sum(item => item.Quantity) ?? 0;
            return Json(cartCount);
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

        [HttpGet]
        public IActionResult ClearCart()
        {
            HttpContext.Session.Remove("Cart");
            return RedirectToAction("Index");
        }
    }
}
