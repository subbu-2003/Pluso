using Pluso.Api.Model;
using Pluso.Api.Repositorys;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace OncallActingDriver._Services
{
    public class RazorpayService
    {
        private readonly IRazorpay_VerificationRepository _verificationRepository;
        private readonly HttpClient _httpClient;

        private readonly string _razorpayKeyId = "rzp_live_Re0pQmSxBPlZ3v";
        private readonly string _razorpayKeySecret = "14eXgHeOo0l7Gw864Mmn7DTQ";

        public RazorpayService(IRazorpay_VerificationRepository verificationRepository, IHttpClientFactory httpClientFactory)
        {
            _verificationRepository = verificationRepository;
            _httpClient = httpClientFactory.CreateClient();
        }
        // ✅ 1️⃣ Create Razorpay Order
        public async Task<string?> CreateOrderAsync(decimal amountInRupees, int tenantId)
        {
            try
            {
                var orderData = new
                {
                    amount = (int)(amountInRupees * 100), // Razorpay expects amount in paise
                    currency = "INR",
                    receipt = $"tenantId_{tenantId}_{DateTime.UtcNow.Ticks}",
                    payment_capture = 1
                };

                var content = new StringContent(JsonSerializer.Serialize(orderData), Encoding.UTF8, "application/json");
                var authToken = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_razorpayKeyId}:{_razorpayKeySecret}"));
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authToken);

                var response = await _httpClient.PostAsync("https://api.razorpay.com/v1/orders", content);
                var responseString = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    throw new Exception($"Razorpay Error: {responseString}");

                var jsonDoc = JsonDocument.Parse(responseString);
                return jsonDoc.RootElement.GetProperty("id").GetString();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ CreateOrder Error: {ex.Message}");
                return null;
            }
        }

        // ✅ 2️⃣ Verify & Capture Razorpay Payment
        public async Task<Razorpay_Verification> VerifyAndCapturePaymentAsync(
            string orderId,
            string paymentId,
            string signature,
            int tenantId,
            decimal amountInRupees,
            string currency)
        {
            var verification = new Razorpay_Verification
            {
                RazorpayOrderId = orderId,
                RazorpayPaymentId = paymentId,
                RazorpaySignature = signature,
                TenantId = tenantId,
                Amount = amountInRupees,
                Currency = currency,
                CreatedAt = DateTime.UtcNow,
                VerifiedAt = DateTime.UtcNow
            };

            try
            {
                string payload = $"{orderId}|{paymentId}";
                string expectedSignature = GenerateHmacSha256(payload, _razorpayKeySecret);

                if (!expectedSignature.Equals(signature, StringComparison.OrdinalIgnoreCase))
                {
                    verification.Status = "FAILED";
                    verification.ResponseJson = JsonSerializer.Serialize(new { message = "Invalid Razorpay signature" });
                }
                else
                {
                    verification.Status = "CAPTURED";
                    verification.CapturedAt = DateTime.UtcNow;
                    verification.ResponseJson = JsonSerializer.Serialize(new { message = "Payment verified and captured" });
                }

                await _verificationRepository.AddVerificationAsync(verification);
            }
            catch (Exception ex)
            {
                verification.Status = "ERROR";
                verification.ResponseJson = JsonSerializer.Serialize(new { message = ex.Message });
                await _verificationRepository.AddVerificationAsync(verification);
            }
            return verification;
        }

        private static string GenerateHmacSha256(string data, string secret)
        {
            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secret));
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }
    }
}
