using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Reina.MacCredy.Extensions;
using Reina.MacCredy.Models;
using Reina.MacCredy.Repositories;

namespace Reina.MacCredy.Controllers
{
    // Remove class-level Authorize attribute to allow some methods to be accessed by guests
    public class ShoppingCartController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        
        public ShoppingCartController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IProductRepository productRepository)
        {
            _productRepository = productRepository;
            _context = context;
            _userManager = userManager;
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
            if (User.Identity.IsAuthenticated)
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
                // Handle empty cart...
                return RedirectToAction("Index");
            }
            
            // For logged in users
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    order.UserId = user.Id;
                }
            }
            else
            {
                // For guest users, ensure we have minimum required information
                if (string.IsNullOrEmpty(order.ShippingAddress) || string.IsNullOrEmpty(order.Email))
                {
                    ModelState.AddModelError("", "Please provide shipping address and email");
                    return View("GuestCheckout", order);
                }
                
                // Set guest flag
                order.IsGuestOrder = true;
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

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            HttpContext.Session.Remove("Cart");
            return View("OrderCompleted", order.Id); // Order completion confirmation page
        }

        [HttpGet]
        public async Task<IActionResult> AddToCart(int productId, int quantity = 1)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null)
            {
                return NotFound();
            }

            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart") ?? new ShoppingCart();
            
            var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == productId);
            if (existingItem != null)
            {
                // For direct quantity updates (not incremental)
                if (quantity <= 0)
                {
                    cart.RemoveItem(productId);
                }
                else
                {
                    existingItem.Quantity = quantity;
                }
            }
            else if (quantity > 0)
            {
                cart.AddItem(product, quantity);
            }

            HttpContext.Session.SetObjectAsJson("Cart", cart);
            
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { 
                    success = true, 
                    message = "Cart updated successfully",
                    quantity = quantity <= 0 ? 1 : quantity
                });
            }
            
            // Check if user is authenticated before proceeding to cart
            if (!User.Identity.IsAuthenticated)
            {
                // Store return URL for after login
                return RedirectToAction("LoginPrompt", new { returnUrl = Url.Action("Index", "ShoppingCart") });
            }
            
            return RedirectToAction("Index");
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
        public IActionResult UpdateQuantity(int productId, int quantity)
        {
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart") ?? new ShoppingCart();
            var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);
            
            if (item != null)
            {
                item.Quantity = Math.Max(1, quantity); // Ensure quantity is at least 1
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
