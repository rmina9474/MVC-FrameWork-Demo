using System.Text;
using System.Text.Json;
using Reina.MacCredy.Models;

namespace Reina.MacCredy.Services
{
    public interface IPaymentService
    {
        Task<(bool success, string redirectUrl, string message)> ProcessMoMoPaymentAsync(Order order);
        Task<(bool success, string redirectUrl, string message)> ProcessVNPayPaymentAsync(Order order);
        Task<(bool success, string message)> VerifyPaymentCallbackAsync(string provider, Dictionary<string, string> callbackParams);
    }

    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<PaymentService> _logger;
        private readonly HttpClient _httpClient;

        public PaymentService(IConfiguration configuration, ILogger<PaymentService> logger, HttpClient httpClient)
        {
            _configuration = configuration;
            _logger = logger;
            _httpClient = httpClient;
        }

        public async Task<(bool success, string redirectUrl, string message)> ProcessMoMoPaymentAsync(Order order)
        {
            try
            {
                // These would be loaded from configuration in a real application
                string momoEndpoint = _configuration["Payment:MoMo:Endpoint"] ?? "https://test-payment.momo.vn/v2/gateway/api/create";
                string partnerCode = _configuration["Payment:MoMo:PartnerCode"] ?? "MOMO";
                string accessKey = _configuration["Payment:MoMo:AccessKey"] ?? "F8BBA842ECF85";
                string secretKey = _configuration["Payment:MoMo:SecretKey"] ?? "K951B6PE1waDMi640xX08PD3vg6EkVlz";
                string returnUrl = _configuration["Payment:MoMo:ReturnUrl"] ?? "http://localhost:5224/ShoppingCart/MoMoCallback";
                string notifyUrl = _configuration["Payment:MoMo:NotifyUrl"] ?? "http://localhost:5224/api/payment/momo-notify";

                string orderId = $"ORDER_{order.Id}_{DateTime.Now.Ticks}";
                string requestId = Guid.NewGuid().ToString();
                string amount = Convert.ToInt32(order.TotalPrice).ToString();
                string orderInfo = $"Payment for order #{order.Id}";

                // Create signature (this is just a simplified example)
                string rawSignature = $"accessKey={accessKey}&amount={amount}&extraData=&ipnUrl={notifyUrl}&orderId={orderId}&orderInfo={orderInfo}&partnerCode={partnerCode}&redirectUrl={returnUrl}&requestId={requestId}&requestType=captureWallet";
                string signature = ComputeHmacSha256(rawSignature, secretKey);

                var requestData = new
                {
                    partnerCode,
                    requestId,
                    amount,
                    orderId,
                    orderInfo,
                    redirectUrl = returnUrl,
                    ipnUrl = notifyUrl,
                    extraData = "",
                    requestType = "captureWallet",
                    signature,
                    lang = "vi"
                };

                var jsonContent = JsonSerializer.Serialize(requestData);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(momoEndpoint, content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var responseData = JsonSerializer.Deserialize<JsonElement>(responseContent);
                    if (responseData.TryGetProperty("payUrl", out var payUrlElement))
                    {
                        string payUrl = payUrlElement.GetString() ?? "";
                        // In a real implementation, you would save the relevant data to the Order record
                        return (true, payUrl, "MoMo payment initiated successfully");
                    }
                }

                _logger.LogError("MoMo payment failed: {Response}", responseContent);
                return (false, "", $"Failed to process MoMo payment: {responseContent}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing MoMo payment");
                return (false, "", $"Error processing payment: {ex.Message}");
            }
        }

        public async Task<(bool success, string redirectUrl, string message)> ProcessVNPayPaymentAsync(Order order)
        {
            try
            {
                // These would be loaded from configuration in a real application
                string vnpayUrl = _configuration["Payment:VNPay:PaymentUrl"] ?? "https://sandbox.vnpayment.vn/paymentv2/vpcpay.html";
                string returnUrl = _configuration["Payment:VNPay:ReturnUrl"] ?? "http://localhost:5224/ShoppingCart/VNPayCallback";
                string tmnCode = _configuration["Payment:VNPay:TmnCode"] ?? "1SNJ89L8";
                string hashSecret = _configuration["Payment:VNPay:HashSecret"] ?? "ODJLXOCEWMFGJXKYZTUGCJPBMHUMGRMW";

                string orderInfo = $"Payment for order #{order.Id}";
                string orderId = $"{DateTime.Now.Ticks}_{order.Id}";

                var vnpParams = new Dictionary<string, string>();
                vnpParams.Add("vnp_Version", "2.1.0");
                vnpParams.Add("vnp_Command", "pay");
                vnpParams.Add("vnp_TmnCode", tmnCode);
                vnpParams.Add("vnp_Locale", "vn");
                vnpParams.Add("vnp_CurrCode", "VND");
                vnpParams.Add("vnp_TxnRef", orderId);
                vnpParams.Add("vnp_OrderInfo", orderInfo);
                vnpParams.Add("vnp_OrderType", "other");
                vnpParams.Add("vnp_Amount", (Convert.ToInt32(order.TotalPrice) * 100).ToString());
                vnpParams.Add("vnp_ReturnUrl", returnUrl);
                vnpParams.Add("vnp_IpAddr", "127.0.0.1");
                vnpParams.Add("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));

                var sortedParams = vnpParams.OrderBy(kvp => kvp.Key).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
                var stringBuilder = new StringBuilder();

                foreach (var param in sortedParams)
                {
                    stringBuilder.Append(param.Key + "=" + param.Value + "&");
                }

                // Remove the trailing '&'
                stringBuilder.Remove(stringBuilder.Length - 1, 1);

                string hashData = stringBuilder.ToString();
                string secureHash = ComputeHmacSha512(hashData, hashSecret);

                vnpParams.Add("vnp_SecureHash", secureHash);

                string queryString = string.Join("&", vnpParams.Select(kvp => $"{kvp.Key}={Uri.EscapeDataString(kvp.Value)}"));
                string paymentUrl = $"{vnpayUrl}?{queryString}";

                // In a real implementation, you would save the relevant data to the Order record
                await Task.Delay(1); // Adding an await operation to make this truly async
                return (true, paymentUrl, "VNPay payment initiated successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing VNPay payment");
                return (false, "", $"Error processing payment: {ex.Message}");
            }
        }

        public async Task<(bool success, string message)> VerifyPaymentCallbackAsync(string provider, Dictionary<string, string> callbackParams)
        {
            try
            {
                await Task.Delay(1); // Adding an await operation to make this truly async
                switch (provider.ToLower())
                {
                    case "momo":
                        return VerifyMoMoCallback(callbackParams);
                    case "vnpay":
                        return VerifyVNPayCallback(callbackParams);
                    default:
                        return (false, $"Unsupported payment provider: {provider}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error verifying {Provider} payment callback", provider);
                return (false, $"Error verifying payment: {ex.Message}");
            }
        }

        private (bool success, string message) VerifyMoMoCallback(Dictionary<string, string> callbackParams)
        {
            // In a real implementation, you would:
            // 1. Verify the signature from MoMo
            // 2. Check if the transaction was successful
            // 3. Update the order with payment information

            if (callbackParams.TryGetValue("resultCode", out string? resultCode))
            {
                if (resultCode == "0")
                {
                    return (true, "MoMo payment successful");
                }
                else if (resultCode == "1006")
                {
                    return (false, "Payment was cancelled by the user");
                }
                else
                {
                    return (false, $"MoMo payment failed with code {resultCode}");
                }
            }

            return (false, "MoMo payment failed - missing result code");
        }

        private (bool success, string message) VerifyVNPayCallback(Dictionary<string, string> callbackParams)
        {
            // In a real implementation, you would:
            // 1. Verify the secure hash from VNPay
            // 2. Check if the transaction was successful
            // 3. Update the order with payment information

            if (callbackParams.TryGetValue("vnp_ResponseCode", out string? responseCode))
            {
                if (responseCode == "00")
                {
                    return (true, "VNPay payment successful");
                }
                else if (responseCode == "24")
                {
                    return (false, "Payment was cancelled by the user");
                }
                else
                {
                    return (false, $"VNPay payment failed with code {responseCode}");
                }
            }

            return (false, "VNPay payment failed - missing response code");
        }

        private string ComputeHmacSha256(string data, string secretKey)
        {
            var keyBytes = Encoding.UTF8.GetBytes(secretKey);
            var dataBytes = Encoding.UTF8.GetBytes(data);

            using (var hmac = new System.Security.Cryptography.HMACSHA256(keyBytes))
            {
                var hashBytes = hmac.ComputeHash(dataBytes);
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }

        private string ComputeHmacSha512(string data, string secretKey)
        {
            var keyBytes = Encoding.UTF8.GetBytes(secretKey);
            var dataBytes = Encoding.UTF8.GetBytes(data);

            using (var hmac = new System.Security.Cryptography.HMACSHA512(keyBytes))
            {
                var hashBytes = hmac.ComputeHash(dataBytes);
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }
    }
}