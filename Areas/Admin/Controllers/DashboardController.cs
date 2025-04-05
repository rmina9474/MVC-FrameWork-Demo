using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reina.MacCredy.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Reina.MacCredy.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Create a dashboard view model with summary stats
            var totalProducts = await _context.Products.CountAsync();
            var totalCategories = await _context.Categories.CountAsync();
            var totalOrders = await _context.Orders.CountAsync();

            // Calculate monthly revenue
            var currentMonth = DateTime.Now.Month;
            var currentYear = DateTime.Now.Year;
            var monthlyRevenue = await _context.Orders
                .Where(o => o.OrderDate.Month == currentMonth && o.OrderDate.Year == currentYear)
                .SumAsync(o => o.TotalPrice);

            // Modified query to safely get recent orders without using new columns
            var recentOrderIds = await _context.Orders
                .OrderByDescending(o => o.OrderDate)
                .Take(5)
                .Select(o => o.Id)
                .ToListAsync();
            // Now get the full order objects safely
            var recentOrders = new List<OrderDisplayModel>();
            foreach (var orderId in recentOrderIds)
            {
                // Execute raw SQL to get only existing columns
                var order = await _context.Orders
                    .Where(o => o.Id == orderId)
                    .Select(o => new OrderDisplayModel
                    {
                        Id = o.Id,
                        OrderDate = o.OrderDate,
                        TotalPrice = o.TotalPrice,
                        ShippingAddress = o.ShippingAddress ?? "",
                        Notes = o.Notes ?? "",
                        UserId = o.UserId ?? "",
                        UserName = "Unknown" // Initialize with default value
                    })
                    .FirstOrDefaultAsync();
                if (order != null)
                {
                    // Get the username if UserId exists
                    if (!string.IsNullOrEmpty(order.UserId))
                    {
                        var user = await _context.Users.FindAsync(order.UserId);
                        order.UserName = user?.UserName ?? "Unknown";
                    }
                    else
                    {
                        order.UserName = "Guest";
                    }
                    recentOrders.Add(order);
                }
            }

            ViewBag.TotalProducts = totalProducts;
            ViewBag.TotalCategories = totalCategories;
            ViewBag.TotalOrders = totalOrders;
            ViewBag.MonthlyRevenue = monthlyRevenue;
            ViewBag.RecentOrders = recentOrders;

            return View();
        }
    }

    // Simple model to hold only the fields we need for display
    public class OrderDisplayModel
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        public required string ShippingAddress { get; set; }
        public required string Notes { get; set; }
        public required string UserId { get; set; }
        public required string UserName { get; set; }
    }
}