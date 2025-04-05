using Reina.MacCredy.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Reina.MacCredy.Extensions;
using Reina.MacCredy.Repositories;

namespace Reina.MacCredy.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public OrderController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IProductRepository productRepository)
        {
            _productRepository = productRepository;
            _context = context;
            _userManager = userManager;
        }

        private async Task<bool> IsUserAdmin()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                return await _userManager.IsInRoleAsync(user, "Admin");
            }
            return false;
        }

        [HttpGet]
        [Route("Order")]
        [Route("Order/Index")]
        public IActionResult Index()
        {
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart") ?? new ShoppingCart();
            return View("CurrentOrder", cart);
        }

        [HttpGet]
        [Authorize]
        [Route("Order/History")]
        public async Task<IActionResult> OrderHistory()
        {
            if (await IsUserAdmin())
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }

            var userId = await _userManager.GetUserIdAsync(await _userManager.GetUserAsync(User));   
            var orderIds = await _context.Orders
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.OrderDate)
                .Select(o => o.Id)
                .ToListAsync();

            var ordersList = new List<OrderViewModel>();
            foreach (var orderId in orderIds)
            {
                var order = await _context.Orders
                    .Where(o => o.Id == orderId)
                    .Select(o => new OrderViewModel
                    {
                        Id = o.Id,
                        OrderDate = o.OrderDate,
                        TotalPrice = o.TotalPrice,
                        ShippingAddress = o.ShippingAddress,
                        UserId = o.UserId
                    })
                    .FirstOrDefaultAsync();

                if (order != null)
                {
                    var orderDetailCount = await _context.OrderDetails
                        .CountAsync(od => od.OrderId == orderId);                
                    order.OrderItemCount = orderDetailCount;
                    ordersList.Add(order);
                }
            }

            return View("History", ordersList);
        }

        public async Task<IActionResult> Details(int id)
        {
            if (await IsUserAdmin())
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }

            var userId = await _userManager.GetUserIdAsync(await _userManager.GetUserAsync(User));
            var orderExists = await _context.Orders
                .AnyAsync(o => o.Id == id && o.UserId == userId);

            if (!orderExists)
            {
                return NotFound();
            }

            var orderInfo = await _context.Orders
                .Where(o => o.Id == id && o.UserId == userId)
                .Select(o => new OrderViewModel
                {
                    Id = o.Id,
                    OrderDate = o.OrderDate,
                    TotalPrice = o.TotalPrice,
                    ShippingAddress = o.ShippingAddress,
                    Notes = o.Notes,
                    UserId = o.UserId
                })
                .FirstOrDefaultAsync();

            if (orderInfo == null)
            {
                return NotFound();
            }

            orderInfo.OrderDetails = await _context.OrderDetails
                .Where(od => od.OrderId == id)
                .Include(od => od.Product)
                .ToListAsync();

            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                orderInfo.UserName = user.UserName;
                orderInfo.Email = user.Email;
                orderInfo.PhoneNumber = user.PhoneNumber;
            }

            return View(orderInfo);
        }

        public async Task<IActionResult> Checkout()
        {
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");
            if (cart == null || !cart.Items.Any())
            {
                return RedirectToAction("Index", "Home");
            }
            
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
            
            return View("GuestCheckout", new Order
            {
                OrderDetails = new List<OrderDetail>()
            });
        }
        
        [HttpPost]
        public async Task<IActionResult> Checkout(Order order)
        {
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");
            if (cart == null || !cart.Items.Any())
            {
                return RedirectToAction("Index");
            }
            
            if (User?.Identity != null && User.Identity.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    order.UserId = user.Id;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(order.ShippingAddress) || string.IsNullOrEmpty(order.Email))
                {
                    ModelState.AddModelError("", "Please provide shipping address and email");
                    return View("GuestCheckout", order);
                }
                
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

            if (string.IsNullOrEmpty(order.Notes))
            {
                order.Notes = "No additional notes.";
            }

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            HttpContext.Session.Remove("Cart");
            return View("OrderCompleted", order.Id);
        }
        
        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, int quantity = 1, decimal totalPrice = 0, string selectedOptions = "")
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null)
            {
                return NotFound();
            }

            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart") ?? new ShoppingCart();
            
            decimal actualPrice = totalPrice > 0 ? totalPrice : product.Price;
            
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

            HttpContext.Session.SetObjectAsJson("Cart", cart);
            
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { 
                    success = true, 
                    message = "Order updated successfully",
                    quantity = quantity
                });
            }
            
            if (User?.Identity == null || !User.Identity.IsAuthenticated)
            {
                return RedirectToAction("LoginPrompt", new { returnUrl = Url.Action("Index", "Order") });
            }
            
            return RedirectToAction("Index");
        }
        
        [HttpGet("Order/RemoveItem")]
        public IActionResult RemoveItem(int productId)
        {
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");
            if (cart != null)
            {
                cart.RemoveItem(productId);
                HttpContext.Session.SetObjectAsJson("Cart", cart);
            }

            return RedirectToAction("Index", "Order");
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
                item.Quantity = Math.Max(1, Math.Min(10, quantity));
                HttpContext.Session.SetObjectAsJson("Cart", cart);
                return Json(new { success = true });
            }
            
            return Json(new { success = false });
        }
        
        public IActionResult LoginPrompt(string returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl ?? Url.Action("Checkout", "Order");
            return View();
        }
        
        [Authorize]
        public async Task<IActionResult> TransferGuestCart()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            
            var sessionCart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");
            if (sessionCart == null || !sessionCart.Items.Any())
            {
                return RedirectToAction("Index");
            }
            
            return RedirectToAction("Index");
        }
    }

    public class OrderViewModel
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        public string ShippingAddress { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public List<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
        public int OrderItemCount { get; set; }
    }
} 