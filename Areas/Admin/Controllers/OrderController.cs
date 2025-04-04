using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reina.MacCredy.Models;
using System.Threading.Tasks;
using System.Linq;
using System;
using System.Collections.Generic;

namespace Reina.MacCredy.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Hiển thị danh sách đơn hàng
        public async Task<IActionResult> Index()
        {
            try 
            {
                // Use a simplified query that only selects fields that exist in the database
                var orderIds = await _context.Orders
                    .OrderByDescending(o => o.OrderDate)
                    .Select(o => o.Id)
                    .ToListAsync();
                
                var orderList = new List<OrderViewModel>();
                
                foreach (var id in orderIds)
                {
                    var order = await _context.Orders
                        .Where(o => o.Id == id)
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
                    
                    if (order != null)
                    {
                        // Get user information separately if needed
                        if (!string.IsNullOrEmpty(order.UserId))
                        {
                            var user = await _context.Users
                                .Where(u => u.Id == order.UserId)
                                .Select(u => new { u.UserName, u.Email })
                                .FirstOrDefaultAsync();
                            
                            if (user != null)
                            {
                                order.UserName = user.UserName ?? "Unknown";
                                order.Email = user.Email ?? "";
                            }
                        }
                        
                        // Get order details count
                        order.OrderItemCount = await _context.OrderDetails
                            .CountAsync(od => od.OrderId == id);
                        
                        orderList.Add(order);
                    }
                }
                
                return View(orderList);
            }
            catch (Exception ex)
            {
                // Log the exception details
                Console.WriteLine($"Error in Admin OrderController.Index: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
                
                // Return a friendly error view
                return View("Error", new ErrorViewModel 
                { 
                    RequestId = System.Diagnostics.Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    ErrorMessage = "There was an error accessing the orders. The database schema might be out of sync."
                });
            }
        }

        // Hiển thị chi tiết đơn hàng
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                // Use a simplified query approach
                var order = await _context.Orders
                    .Where(o => o.Id == id)
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

                if (order == null)
                {
                    return NotFound();
                }
                
                // Get user information separately
                if (!string.IsNullOrEmpty(order.UserId))
                {
                    var user = await _context.Users
                        .Where(u => u.Id == order.UserId)
                        .Select(u => new { u.UserName, u.Email, u.PhoneNumber })
                        .FirstOrDefaultAsync();
                    
                    if (user != null)
                    {
                        order.UserName = user.UserName ?? "Unknown";
                        order.Email = user.Email ?? "";
                        order.PhoneNumber = user.PhoneNumber ?? "";
                    }
                }
                
                // Get order details separately
                order.OrderDetails = await _context.OrderDetails
                    .Where(od => od.OrderId == id)
                    .Include(od => od.Product)
                    .ToListAsync();

                return View(order);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error loading order details: {ex.Message}");
                return View("Error", new ErrorViewModel { RequestId = "Error loading order details" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int id, string status)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            // Add status update logic here when you implement order status
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { id });
        }
        
        // Delete order action
        public async Task<IActionResult> Delete(int id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderDetails)
                .FirstOrDefaultAsync(o => o.Id == id);
                
            if (order == null)
            {
                return NotFound();
            }
            
            try
            {
                // First remove all the order details
                _context.OrderDetails.RemoveRange(order.OrderDetails);
                
                // Then remove the order itself
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
                
                TempData["Success"] = "Order has been deleted successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error deleting order: {ex.Message}";
                return RedirectToAction(nameof(Details), new { id });
            }
        }
    }
    
    // View model to match only existing database columns
    public class OrderViewModel
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        public string ShippingAddress { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
        public string? UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public List<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
        public int OrderItemCount { get; set; }
    }
}
