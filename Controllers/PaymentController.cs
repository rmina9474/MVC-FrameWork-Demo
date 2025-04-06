using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reina.MacCredy.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Collections.Generic;

namespace Reina.MacCredy.Controllers
{
    public class PaymentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PaymentController> _logger;

        public PaymentController(ApplicationDbContext context, ILogger<PaymentController> logger)
        {
            _context = context;
            _logger = logger;
        }
        
        // Action method to display MoMo QR code for payment
        public IActionResult MoMoQR(string orderId, long amount)
        {
            _logger.LogInformation($"Generating MoMo QR page for orderId: {orderId}, amount: {amount}");
            
            if (string.IsNullOrEmpty(orderId))
            {
                return RedirectToAction("Index", "Home");
            }
            
            // Extract actual order ID from the MoMo order reference
            string[] orderIdParts = orderId.Split('_');
            if (orderIdParts.Length >= 2 && int.TryParse(orderIdParts[1], out int actualOrderId))
            {
                // Look up the order from database
                var order = _context.Orders.FirstOrDefault(o => o.Id == actualOrderId);
                
                if (order == null)
                {
                    _logger.LogWarning($"Order {actualOrderId} not found in database");
                    return RedirectToAction("Index", "Home");
                }
                
                // Prepare view model with order details
                ViewBag.OrderId = orderId;
                ViewBag.Amount = amount;
                ViewBag.OrderReference = order.Id;
                ViewBag.OrderDate = order.OrderDate;
                
                // Generate QR code data - in a real app, this would be the MoMo-specific payload
                string qrData = $"momo://pay?phone=01234567890&amount={amount}&note=Order{order.Id}";
                _logger.LogInformation($"Generated QR data: {qrData}");
                
                // Since we're having issues with QRCoder, we'll generate a demo image URL instead
                // In a real app, we'd use the QRCoder library or get a QR code from MoMo
                string demoQrUrl = $"https://api.qrserver.com/v1/create-qr-code/?size=200x200&data={Uri.EscapeDataString(qrData)}";
                ViewBag.QRCodeImage = demoQrUrl;
                
                return View();
            }
            
            return RedirectToAction("Index", "Home");
        }
        
        // Action to handle callback from MoMo after payment is completed
        [HttpPost]
        public async Task<IActionResult> MoMoCallback([FromForm] Dictionary<string, string> formData)
        {
            _logger.LogInformation($"MoMo callback received: {string.Join(", ", formData.Select(kv => $"{kv.Key}={kv.Value}"))}");
            
            // Extract key parameters from the callback
            string orderId = formData.ContainsKey("orderId") ? formData["orderId"] : string.Empty;
            string resultCode = formData.ContainsKey("resultCode") ? formData["resultCode"] : string.Empty;
            string message = formData.ContainsKey("message") ? formData["message"] : string.Empty;
            string transId = formData.ContainsKey("transId") ? formData["transId"] : string.Empty;
            string signature = formData.ContainsKey("signature") ? formData["signature"] : string.Empty;
            
            if (string.IsNullOrEmpty(orderId))
            {
                _logger.LogWarning("MoMo callback missing orderId");
                return BadRequest("Invalid order ID");
            }
            
            // Extract actual order ID from the MoMo order reference
            string[] orderIdParts = orderId.Split('_');
            if (orderIdParts.Length >= 2 && int.TryParse(orderIdParts[1], out int actualOrderId))
            {
                // Look up the order from database
                var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == actualOrderId);
                
                if (order == null)
                {
                    _logger.LogWarning($"Order {actualOrderId} not found in database during callback");
                    return NotFound("Order not found");
                }
                
                // Verify that this orderId matches what we stored in PaymentReference
                if (order.PaymentReference != orderId)
                {
                    _logger.LogWarning($"Order reference mismatch for order {actualOrderId}. Expected: {order.PaymentReference}, Got: {orderId}");
                    return BadRequest("Order reference mismatch");
                }
                
                // In a production environment, we would verify the signature here
                // using the same HMAC-SHA256 algorithm and secret key
                
                // Update order payment status based on result code
                if (resultCode == "0")
                {
                    // Payment successful
                    order.PaymentStatus = SD.PaymentStatus_Approved;
                    order.PaymentResponse = $"Payment successfully processed via MoMo. Transaction ID: {transId}";
                    order.TransactionId = transId;
                    _logger.LogInformation($"Payment for order {actualOrderId} succeeded with transaction {transId}");
                }
                else
                {
                    // Payment failed
                    order.PaymentStatus = SD.PaymentStatus_Rejected;
                    order.PaymentResponse = $"Payment failed: {message}";
                    _logger.LogWarning($"Payment for order {actualOrderId} failed: {resultCode} - {message}");
                }
                
                await _context.SaveChangesAsync();
                
                // MoMo expects a JSON response to acknowledge receipt
                return Json(new 
                { 
                    status = "ok",
                    message = "Callback processed successfully"
                });
            }
            
            return BadRequest("Invalid order format");
        }
        
        // Action to manually mark a payment as complete (for demo purposes)
        public async Task<IActionResult> CompletePayment(string orderId)
        {
            if (string.IsNullOrEmpty(orderId))
            {
                return RedirectToAction("Index", "Home");
            }
            
            // Extract actual order ID from the MoMo order reference
            string[] orderIdParts = orderId.Split('_');
            if (orderIdParts.Length >= 2 && int.TryParse(orderIdParts[1], out int actualOrderId))
            {
                var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == actualOrderId);
                
                if (order != null)
                {
                    order.PaymentStatus = SD.PaymentStatus_Approved;
                    order.PaymentResponse = "Payment manually completed in demo mode";
                    await _context.SaveChangesAsync();
                    
                    return RedirectToAction("OrderCompleted", "ShoppingCart", new { id = actualOrderId });
                }
            }
            
            return RedirectToAction("Index", "Home");
        }

        // Action to check payment status (for polling)
        public async Task<IActionResult> CheckPaymentStatus(string orderId)
        {
            if (string.IsNullOrEmpty(orderId))
            {
                return Json(new { success = false, message = "Invalid order ID" });
            }
            
            // Extract actual order ID from the MoMo order reference
            string[] orderIdParts = orderId.Split('_');
            if (orderIdParts.Length >= 2 && int.TryParse(orderIdParts[1], out int actualOrderId))
            {
                var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == actualOrderId);
                
                if (order == null)
                {
                    return Json(new { success = false, message = "Order not found" });
                }
                
                string status = order.PaymentStatus switch
                {
                    SD.PaymentStatus_Approved => "approved",
                    SD.PaymentStatus_Rejected => "rejected",
                    SD.PaymentStatus_Refunded => "refunded",
                    _ => "pending"
                };
                
                return Json(new
                {
                    success = true,
                    status = status,
                    message = order.PaymentResponse,
                    redirectUrl = Url.Action("OrderCompleted", "ShoppingCart", new { id = actualOrderId })
                });
            }
            
            return Json(new { success = false, message = "Invalid order format" });
        }

        // Action method to display VNPay payment page
        public IActionResult VNPay(string orderId, long amount)
        {
            _logger.LogInformation($"Generating VNPay payment page for orderId: {orderId}, amount: {amount}");
            
            if (string.IsNullOrEmpty(orderId))
            {
                return RedirectToAction("Index", "Home");
            }
            
            // Extract actual order ID from the VNPay order reference
            string[] orderIdParts = orderId.Split('_');
            if (orderIdParts.Length >= 2 && int.TryParse(orderIdParts[1], out int actualOrderId))
            {
                var order = _context.Orders.FirstOrDefault(o => o.Id == actualOrderId);
                if (order == null)
                {
                    _logger.LogWarning($"Order not found: {actualOrderId}");
                    return RedirectToAction("Index", "Home");
                }
                
                // Format the date for display
                DateTime orderDate = order.OrderDate;
                
                // The payment URL is stored in the PaymentResponse field when creating the request
                string paymentUrl = order.PaymentResponse;
                
                // Validate payment URL
                if (string.IsNullOrEmpty(paymentUrl) || !paymentUrl.StartsWith("http"))
                {
                    _logger.LogWarning($"Invalid payment URL for order {actualOrderId}: {paymentUrl}");
                    TempData["ErrorMessage"] = "Invalid payment URL. Please try again.";
                    return RedirectToAction("Checkout", "ShoppingCart");
                }
                
                // Set up view data for the VNPay page
                ViewBag.OrderReference = orderId;
                ViewBag.OrderId = order.Id;
                ViewBag.Amount = amount / 100; // Convert from VNPay format (amount * 100)
                ViewBag.OrderDate = orderDate;
                ViewBag.PaymentUrl = paymentUrl;
                
                _logger.LogInformation($"Redirecting to VNPay page for order {actualOrderId} with URL: {paymentUrl}");
                
                return View();
            }
            
            return RedirectToAction("Index", "Home");
        }
        
        // Action to handle VNPay callback/return
        [HttpGet]
        public async Task<IActionResult> VNPayReturn(string vnp_TxnRef, string vnp_ResponseCode, string vnp_TransactionStatus, 
            string vnp_OrderInfo, string vnp_PayDate, string vnp_TransactionNo, string vnp_BankCode, 
            string vnp_Amount, string vnp_SecureHash)
        {
            _logger.LogInformation($"VNPay return: TxnRef={vnp_TxnRef}, ResponseCode={vnp_ResponseCode}");
            
            // Extract order ID from transaction reference
            string[] orderIdParts = vnp_TxnRef?.Split('_') ?? Array.Empty<string>();
            if (orderIdParts.Length >= 2 && int.TryParse(orderIdParts[1], out int orderId))
            {
                var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == orderId);
                if (order == null)
                {
                    TempData["ErrorMessage"] = "Order not found";
                    return RedirectToAction("Index", "Home");
                }
                
                // Check VNPay response code - 00 means success
                if (vnp_ResponseCode == "00" && vnp_TransactionStatus == "00")
                {
                    // Update order with payment success
                    order.PaymentStatus = SD.PaymentStatus_Approved;
                    order.PaymentResponse = $"VNPay payment completed: {vnp_TransactionNo} via {vnp_BankCode}";
                    await _context.SaveChangesAsync();
                    
                    TempData["SuccessMessage"] = "Payment completed successfully!";
                }
                else
                {
                    // Update order with payment failure
                    order.PaymentStatus = SD.PaymentStatus_Rejected;
                    order.PaymentResponse = $"VNPay payment failed: ResponseCode={vnp_ResponseCode}, Status={vnp_TransactionStatus}";
                    await _context.SaveChangesAsync();
                    
                    TempData["ErrorMessage"] = "Payment was not successful. Please try again.";
                }
                
                // Redirect to order completion page
                return RedirectToAction("OrderCompleted", "ShoppingCart", new { id = orderId });
            }
            
            TempData["ErrorMessage"] = "Invalid payment information";
            return RedirectToAction("Index", "Home");
        }

        // Action method to handle VNPay IPN (Instant Payment Notification)
        [HttpGet]
        public async Task<IActionResult> VNPayIPN(string vnp_TxnRef, string vnp_TransactionStatus, 
            string vnp_ResponseCode, string vnp_SecureHash)
        {
            _logger.LogInformation($"VNPay IPN: TxnRef={vnp_TxnRef}, Status={vnp_TransactionStatus}");
            
            // Extract order ID from transaction reference
            string[] orderIdParts = vnp_TxnRef?.Split('_') ?? Array.Empty<string>();
            if (orderIdParts.Length >= 2 && int.TryParse(orderIdParts[1], out int orderId))
            {
                var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == orderId);
                if (order == null)
                {
                    return Json(new { vnp_ResponseCode = "01", vnp_Message = "Order not found" });
                }
                
                // Validate the response by checking vnp_SecureHash (in a real implementation)
                // For demo, we'll just check response code
                if (vnp_ResponseCode == "00" && vnp_TransactionStatus == "00")
                {
                    // Update order with payment success
                    order.PaymentStatus = SD.PaymentStatus_Approved;
                    order.PaymentResponse = $"VNPay payment completed via IPN";
                    await _context.SaveChangesAsync();
                    
                    return Json(new { vnp_ResponseCode = "00", vnp_Message = "Confirm success" });
                }
                else
                {
                    // Update order with payment failure
                    order.PaymentStatus = SD.PaymentStatus_Rejected;
                    order.PaymentResponse = $"VNPay payment failed: ResponseCode={vnp_ResponseCode}, Status={vnp_TransactionStatus}";
                    await _context.SaveChangesAsync();
                    
                    return Json(new { vnp_ResponseCode = "00", vnp_Message = "Confirm success" });
                }
            }
            
            return Json(new { vnp_ResponseCode = "01", vnp_Message = "Order not found" });
        }
    }
} 