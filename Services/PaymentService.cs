using Reina.MacCredy.Models;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Text;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Linq;
using System.Net;
using System.Web;

namespace Reina.MacCredy.Services
{
    public class PaymentService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<PaymentService> _logger;
        private readonly ApplicationDbContext _context;

        public PaymentService(IConfiguration configuration, ILogger<PaymentService> logger, ApplicationDbContext context)
        {
            _configuration = configuration;
            _logger = logger;
            _context = context;
        }

        /// <summary>
        /// Process payment based on the selected payment method
        /// </summary>
        /// <param name="order">The order to process payment for</param>
        /// <param name="returnUrl">The URL to return to after payment processing</param>
        /// <returns>Result containing success status, message, and redirect URL if applicable</returns>
        public async Task<PaymentResult> ProcessPaymentAsync(Order order, string returnUrl)
        {
            return order.PaymentMethod switch
            {
                SD.Payment_CashOnDelivery => new PaymentResult
                {
                    Success = true,
                    Message = "Cash on delivery order placed successfully.",
                    RedirectUrl = null
                },
                SD.Payment_CreditCard => await ProcessCreditCardPaymentAsync(order, returnUrl),
                SD.Payment_BankTransfer => await ProcessBankTransferPaymentAsync(order, returnUrl),
                SD.Payment_MoMo => await ProcessMoMoPaymentAsync(order, returnUrl),
                SD.Payment_VNPay => await ProcessVNPayPaymentAsync(order, returnUrl),
                _ => new PaymentResult
                {
                    Success = false,
                    Message = "Invalid payment method selected.",
                    RedirectUrl = null
                }
            };
        }

        private async Task<PaymentResult> ProcessCreditCardPaymentAsync(Order order, string returnUrl)
        {
            // In a real implementation, this would integrate with a payment gateway
            _logger.LogInformation($"Processing credit card payment for order {order.Id}");
            
            // Simulate payment processing
            await Task.Delay(1000);
            
            return new PaymentResult
            {
                Success = true,
                Message = "Credit card payment processed successfully.",
                RedirectUrl = null
            };
        }

        private async Task<PaymentResult> ProcessBankTransferPaymentAsync(Order order, string returnUrl)
        {
            _logger.LogInformation($"Processing bank transfer for order {order.Id}");
            
            // Simulate payment processing
            await Task.Delay(1000);
            
            return new PaymentResult
            {
                Success = true,
                Message = "Bank transfer information provided. Please complete the transfer using the provided instructions.",
                RedirectUrl = null
            };
        }

        private async Task<PaymentResult> ProcessMoMoPaymentAsync(Order order, string returnUrl)
        {
            _logger.LogInformation($"Processing MoMo payment for order {order.Id}");
            
            try
            {
                // Read configuration values
                var partnerCode = _configuration["Payment:MoMo:PartnerCode"];
                var accessKey = _configuration["Payment:MoMo:AccessKey"];
                var secretKey = _configuration["Payment:MoMo:SecretKey"];
                var apiEndpoint = _configuration["Payment:MoMo:ApiEndpoint"];
                
                // For demo purposes, use dummy values if configuration is missing
                if (string.IsNullOrEmpty(partnerCode) || string.IsNullOrEmpty(accessKey) || 
                    string.IsNullOrEmpty(secretKey) || string.IsNullOrEmpty(apiEndpoint))
                {
                    _logger.LogWarning("MoMo configuration missing. Using dummy values for demo");
                    partnerCode = "MOMO";
                    accessKey = "F8BBA842ECF85";
                    secretKey = "K951B6PE1waDMi640xX08PD3vg6EkVlz";
                    apiEndpoint = "https://test-payment.momo.vn/v2/gateway/api/create";
                }
                
                // Generate unique request & order IDs
                var requestId = Guid.NewGuid().ToString();
                var orderId = $"ORD_{order.Id}_{DateTime.Now.Ticks}";
                
                // Save the reference for later verification
                order.PaymentReference = orderId;
                await _context.SaveChangesAsync();
                
                // Create payment request parameters
                var amount = (long)order.TotalPrice;
                var orderInfo = $"Payment for order #{order.Id}";
                
                // Build the raw signature string
                var rawSignature = $"accessKey={accessKey}&amount={amount}&extraData=&ipnUrl={returnUrl}&orderId={orderId}&orderInfo={orderInfo}&partnerCode={partnerCode}&redirectUrl={returnUrl}&requestId={requestId}&requestType=captureWallet";
                
                // Generate HMAC-SHA256 signature
                var hmac = new System.Security.Cryptography.HMACSHA256(System.Text.Encoding.UTF8.GetBytes(secretKey));
                var signatureBytes = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(rawSignature));
                var signature = BitConverter.ToString(signatureBytes).Replace("-", "").ToLower();
                
                // Create request payload
                var requestData = new
                {
                    partnerCode = partnerCode,
                    partnerName = "Reina MacCredy Cafe",
                    storeId = partnerCode,
                    requestId = requestId,
                    amount = amount,
                    orderId = orderId,
                    orderInfo = orderInfo,
                    redirectUrl = returnUrl,
                    ipnUrl = returnUrl, // In production, this should be a separate callback URL
                    lang = "en",
                    extraData = "",
                    requestType = "captureWallet",
                    signature = signature
                };
                
                _logger.LogInformation($"Sending request to MoMo API: {apiEndpoint}");
                
                // Create HTTP client
                using (var httpClient = new HttpClient())
                {
                    // Set content type correctly
                    httpClient.DefaultRequestHeaders.Clear();
                    httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
                    
                    // Serialize request data
                    var jsonRequest = System.Text.Json.JsonSerializer.Serialize(requestData);
                    var content = new StringContent(jsonRequest, System.Text.Encoding.UTF8, "application/json");
                    
                    // Add a timeout to prevent hanging requests
                    httpClient.Timeout = TimeSpan.FromSeconds(30);
                    
                    // Send request to MoMo API
                    var response = await httpClient.PostAsync(apiEndpoint, content);
                    var responseContent = await response.Content.ReadAsStringAsync();
                    
                    _logger.LogInformation($"MoMo API response: {responseContent}");
                    
                    if (response.IsSuccessStatusCode)
                    {
                        try
                        {
                            // Parse the response
                            var momoResponse = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(responseContent);
                            
                            if (momoResponse != null && momoResponse.ContainsKey("payUrl"))
                            {
                                var payUrl = momoResponse["payUrl"]?.ToString();
                                
                                if (!string.IsNullOrEmpty(payUrl))
                                {
                                    _logger.LogInformation($"MoMo payment URL generated: {payUrl}");
                                    
                                    // Update order with pending payment status
                                    order.PaymentStatus = SD.PaymentStatus_Pending;
                                    order.PaymentResponse = "Awaiting payment via MoMo";
                                    await _context.SaveChangesAsync();
                                    
                                    // Return the payment URL for redirection
                                    return new PaymentResult
                                    {
                                        Success = true,
                                        Message = "Please complete the payment on the MoMo page",
                                        RedirectUrl = payUrl
                                    };
                                }
                            }
                            
                            // Check for error code in response
                            if (momoResponse != null && momoResponse.ContainsKey("resultCode"))
                            {
                                var resultCode = momoResponse["resultCode"]?.ToString();
                                var message = momoResponse.ContainsKey("message") ? momoResponse["message"]?.ToString() : "Unknown error";
                                
                                _logger.LogWarning($"MoMo API returned error: {resultCode} - {message}");
                                
                                // Update order status
                                order.PaymentStatus = SD.PaymentStatus_Rejected;
                                order.PaymentResponse = $"MoMo payment failed: {message}";
                                await _context.SaveChangesAsync();
                                
                                return new PaymentResult
                                {
                                    Success = false,
                                    Message = $"Error processing MoMo payment: {message}",
                                    RedirectUrl = null
                                };
                            }
                        }
                        catch (System.Text.Json.JsonException jsonEx)
                        {
                            _logger.LogError(jsonEx, "Error parsing MoMo API response");
                            order.PaymentResponse = "Error parsing payment gateway response";
                            await _context.SaveChangesAsync();
                        }
                    }
                    else
                    {
                        _logger.LogWarning($"MoMo API returned error status code: {response.StatusCode}");
                        order.PaymentStatus = SD.PaymentStatus_Rejected;
                        order.PaymentResponse = $"Payment gateway error: {response.StatusCode}";
                        await _context.SaveChangesAsync();
                    }
                    
                    // For error cases or when testing locally
                    _logger.LogWarning("Could not get MoMo payment URL. Falling back to local QR demo page");
                    
                    // Generate a fallback QR page URL for cases where MoMo API fails
                    var demoQrPageUrl = $"/Payment/MoMoQR?orderId={orderId}&amount={amount}";
                    
                    return new PaymentResult
                    {
                        Success = true,
                        Message = "Please scan the QR code to complete payment",
                        RedirectUrl = demoQrPageUrl
                    };
                }
            }
            catch (HttpRequestException httpEx)
            {
                _logger.LogError(httpEx, $"HTTP error communicating with MoMo API for order {order.Id}");
                
                // Update order status
                order.PaymentStatus = SD.PaymentStatus_Rejected;
                order.PaymentResponse = "Error contacting payment gateway. Please try again later.";
                await _context.SaveChangesAsync();
                
                return new PaymentResult
                {
                    Success = false,
                    Message = "Error processing MoMo payment. Please try again later.",
                    RedirectUrl = null
                };
            }
            catch (TaskCanceledException tcEx)
            {
                _logger.LogError(tcEx, $"Timeout connecting to MoMo API for order {order.Id}");
                
                // Update order status
                order.PaymentStatus = SD.PaymentStatus_Rejected;
                order.PaymentResponse = "Connection to payment gateway timed out. Please try again later.";
                await _context.SaveChangesAsync();
                
                return new PaymentResult
                {
                    Success = false,
                    Message = "Error processing MoMo payment. Please try again later.",
                    RedirectUrl = null
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error processing MoMo payment for order {order.Id}");
                
                // Update order status
                order.PaymentStatus = SD.PaymentStatus_Rejected;
                order.PaymentResponse = "An unexpected error occurred. Please try again later.";
                await _context.SaveChangesAsync();
                
                return new PaymentResult
                {
                    Success = false,
                    Message = "Error processing MoMo payment. Please try again later.",
                    RedirectUrl = null
                };
            }
        }

        private async Task<PaymentResult> ProcessVNPayPaymentAsync(Order order, string returnUrl)
        {
            _logger.LogInformation($"Processing VNPay payment for order {order.Id}");
            
            try
            {
                // Read configuration values
                var tmnCode = _configuration["Payment:VNPay:TmnCode"];
                var hashSecret = _configuration["Payment:VNPay:HashSecret"];
                var apiEndpoint = _configuration["Payment:VNPay:ApiEndpoint"];
                
                // For demo purposes, use sandbox values (these are VNPay sandbox test values)
                if (string.IsNullOrEmpty(tmnCode) || string.IsNullOrEmpty(hashSecret) || 
                    string.IsNullOrEmpty(apiEndpoint))
                {
                    _logger.LogWarning("Using VNPay sandbox configuration for demo purposes");
                    tmnCode = "NCB";  // VNPay test merchant code (NCB bank)
                    hashSecret = "UVMCJECLPUWPXXLLLGWRUXOMTURXPKEL"; // VNPay test hash secret
                    apiEndpoint = "https://sandbox.vnpayment.vn/paymentv2/vpcpay.html";
                }
                
                // Generate unique transaction reference
                var vnpTxnRef = $"ORD_{order.Id}_{DateTime.Now.Ticks}";
                
                // Save the reference for later verification
                order.PaymentReference = vnpTxnRef;
                await _context.SaveChangesAsync();

                // Get current time in Vietnam timezone (GMT+7)
                DateTime vietnamTime;
                try 
                {
                    var vietnamTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Asia/Bangkok"); // Closest to Vietnam time
                    vietnamTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, vietnamTimeZone);
                }
                catch (TimeZoneNotFoundException)
                {
                    // Fallback for systems that don't have the timezone
                    vietnamTime = DateTime.UtcNow.AddHours(7); // Vietnam is UTC+7
                    _logger.LogWarning("Timezone 'Asia/Bangkok' not found, using UTC+7 manually");
                }
                
                // Create VNPay request parameters
                var vnpParams = new Dictionary<string, string>();
                vnpParams.Add("vnp_Version", "2.1.0");
                vnpParams.Add("vnp_Command", "pay");
                vnpParams.Add("vnp_TmnCode", tmnCode);
                vnpParams.Add("vnp_Amount", (Convert.ToInt64(order.TotalPrice) * 100).ToString()); // Amount in VND, multiply by 100
                vnpParams.Add("vnp_CreateDate", vietnamTime.ToString("yyyyMMddHHmmss"));
                vnpParams.Add("vnp_CurrCode", "VND");
                vnpParams.Add("vnp_IpAddr", "127.0.0.1"); // Should be client IP in production
                vnpParams.Add("vnp_Locale", "vn");
                vnpParams.Add("vnp_OrderInfo", $"Thanh toan don hang #{order.Id}"); // Vietnamese text
                vnpParams.Add("vnp_OrderType", "190000"); // Default order type for e-commerce
                vnpParams.Add("vnp_ReturnUrl", returnUrl);
                vnpParams.Add("vnp_TxnRef", vnpTxnRef);
                
                // Format transaction date according to VNPay format (yyyyMMddHHmmss)
                var txnDate = vietnamTime.ToString("yyyyMMddHHmmss");
                vnpParams.Add("vnp_TxnDate", txnDate);
                
                // Build the signature string (sort parameters by key name)
                var sortedParams = vnpParams
                    .Where(kvp => !string.IsNullOrEmpty(kvp.Value) && kvp.Key.StartsWith("vnp_"))
                    .OrderBy(kvp => kvp.Key)
                    .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
                
                // Create the hash data string WITHOUT URL encoding for the signature calculation
                var hashData = string.Join("&", sortedParams.Select(kv => $"{kv.Key}={kv.Value}"));
                _logger.LogInformation($"Hash data: {hashData}");
                
                // Generate HMAC-SHA512 signature
                var hmac = new System.Security.Cryptography.HMACSHA512(System.Text.Encoding.UTF8.GetBytes(hashSecret));
                var signatureBytes = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(hashData));
                var signature = BitConverter.ToString(signatureBytes).Replace("-", "").ToLower();
                
                // Add signature to params
                vnpParams.Add("vnp_SecureHash", signature);
                
                // Now build the payment URL with URL encoding for the actual HTTP request
                var queryString = string.Join("&", vnpParams.Select(kv => $"{kv.Key}={WebUtility.UrlEncode(kv.Value)}"));
                var paymentUrl = $"{apiEndpoint}?{queryString}";
                
                _logger.LogInformation($"VNPay payment URL generated: {paymentUrl}");
                
                // For debugging - save the URL in the order's payment response
                order.PaymentResponse = paymentUrl;
                order.PaymentStatus = SD.PaymentStatus_Pending;
                await _context.SaveChangesAsync();
                
                // Return the payment URL for redirection
                return new PaymentResult
                {
                    Success = true,
                    Message = "Please complete the payment on the VNPay page",
                    RedirectUrl = paymentUrl
                };
            }
            catch (HttpRequestException httpEx)
            {
                _logger.LogError(httpEx, $"HTTP error communicating with VNPay API for order {order.Id}");
                
                // Update order status
                order.PaymentStatus = SD.PaymentStatus_Rejected;
                order.PaymentResponse = "Communication error with payment gateway. Please try again later.";
                await _context.SaveChangesAsync();
                
                return new PaymentResult
                {
                    Success = false,
                    Message = "Error connecting to VNPay. Please try again later.",
                    RedirectUrl = null
                };
            }
            catch (TaskCanceledException tcEx)
            {
                _logger.LogError(tcEx, $"Timeout connecting to VNPay API for order {order.Id}");
                
                // Update order status
                order.PaymentStatus = SD.PaymentStatus_Rejected;
                order.PaymentResponse = "Connection to payment gateway timed out. Please try again later.";
                await _context.SaveChangesAsync();
                
                return new PaymentResult
                {
                    Success = false,
                    Message = "Connection to VNPay timed out. Please try again later.",
                    RedirectUrl = null
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error processing VNPay payment for order {order.Id}");
                
                // Update order status
                order.PaymentStatus = SD.PaymentStatus_Rejected;
                order.PaymentResponse = "An unexpected error occurred. Please try again later.";
                await _context.SaveChangesAsync();
                
                return new PaymentResult
                {
                    Success = false,
                    Message = "Error processing VNPay payment. Please try again later.",
                    RedirectUrl = null
                };
            }
        }
    }

    public class PaymentResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string RedirectUrl { get; set; }
    }
} 