using Reina.MacCredy.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Reina.MacCredy.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public OrderController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
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

        public async Task<IActionResult> Details(int id)
        {
            // Redirect admin users away from this page
            if (await IsUserAdmin())
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }

            var userId = await _userManager.GetUserIdAsync(await _userManager.GetUserAsync(User));
            // Get the order ID first
            var orderExists = await _context.Orders
                .AnyAsync(o => o.Id == id && o.UserId == userId);

            if (!orderExists)
            {
                return NotFound();
            }

            // Get basic order information
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

            // Get order details separately
            orderInfo.OrderDetails = await _context.OrderDetails
                .Where(od => od.OrderId == id)
                .Include(od => od.Product)
                .ToListAsync();

            // Get user information
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                orderInfo.UserName = user.UserName;
                orderInfo.Email = user.Email;
                orderInfo.PhoneNumber = user.PhoneNumber;
            }

            return View(orderInfo);
        }

        public async Task<IActionResult> Index()
        {
            // Redirect admin users away from this page
            if (await IsUserAdmin())
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }

            var userId = await _userManager.GetUserIdAsync(await _userManager.GetUserAsync(User));   
            // Get order IDs first
            var orderIds = await _context.Orders
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.OrderDate)
                .Select(o => o.Id)
                .ToListAsync();

            // Get order information for each ID
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
                    // Get summary of order details
                    var orderDetailCount = await _context.OrderDetails
                        .CountAsync(od => od.OrderId == orderId);                
                    order.OrderItemCount = orderDetailCount;
                    ordersList.Add(order);
                }
            }

            return View(ordersList);
        }
    }

    // View model to display only the fields that exist in the database
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