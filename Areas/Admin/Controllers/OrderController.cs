using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reina.MacCredy.Models;
using System.Threading.Tasks;
using System.Linq;
using System;

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
            var orders = await _context.Orders
                .Include(o => o.ApplicationUser)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
            return View(orders);
        }

        // Hiển thị chi tiết đơn hàng
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var order = await _context.Orders
                    .Include(o => o.ApplicationUser)
                    .Include(o => o.OrderDetails)
                        .ThenInclude(od => od.Product)
                    .FirstOrDefaultAsync(o => o.Id == id);

                if (order == null)
                {
                    return NotFound();
                }

                // Log some debug information
                Console.WriteLine($"Order found with ID: {order.Id}");
                Console.WriteLine($"Customer: {order.ApplicationUser?.FullName ?? "Not loaded"}");
                Console.WriteLine($"Order details count: {order.OrderDetails?.Count() ?? 0}");

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
}
