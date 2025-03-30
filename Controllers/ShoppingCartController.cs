using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Reina.MacCredy.Extensions;
using Reina.MacCredy.Models;
using Reina.MacCredy.Repositories;

namespace Reina.MacCredy.Controllers
{
    [Authorize]
        
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
        
        public async Task<IActionResult> Checkout()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            
            return View(new Order
            {
                UserId = user.Id,
                ApplicationUser = user,
                ShippingAddress = string.Empty,
                OrderDetails = new List<OrderDetail>()
            });
        }
        
        [HttpPost]
        public async Task<IActionResult> Checkout(Order order)
        {
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");
            if (cart == null || !cart.Items.Any())
            {
                // Handle empty cart...
                return RedirectToAction("Index");
            }
            
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            
            order.UserId = user.Id;
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

    }
}
