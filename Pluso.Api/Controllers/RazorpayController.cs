using Microsoft.AspNetCore.Mvc;
using OncallActingDriver._Services;

namespace ecom.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RazorpayController : ControllerBase
    {
        private readonly RazorpayService _razorpayService;

        public RazorpayController(RazorpayService razorpayService)
        {
            _razorpayService = razorpayService;
        }

        // ================================
        // 1️⃣ Create Razorpay Order
        // ================================
        [HttpPost("CreateOrder")]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
        {
            if (request == null || request.Amount <= 0)
                return BadRequest(new { success = false, message = "Invalid order request" });

            var razorpayOrderId = await _razorpayService.CreateOrderAsync(request.Amount, request.TenantId);

            if (razorpayOrderId == null)
                return BadRequest(new { success = false, message = "Failed to create Razorpay order" });

            return Ok(new { success = true, orderId = razorpayOrderId });
        }

        // ================================
        // 2️⃣ Verify & Capture Razorpay Payment
        // ================================
        [HttpPost("VerifyAndCapture")]
        public async Task<IActionResult> VerifyAndCapture([FromBody] RazorpayVerifyRequest request)
        {
            if (request == null ||
                string.IsNullOrWhiteSpace(request.RazorpayOrderId) ||
                string.IsNullOrWhiteSpace(request.RazorpayPaymentId) ||
                string.IsNullOrWhiteSpace(request.RazorpaySignature))
            {
                return BadRequest(new { success = false, message = "Invalid verification data" });
            }

            var result = await _razorpayService.VerifyAndCapturePaymentAsync(
                request.RazorpayOrderId,
                request.RazorpayPaymentId,
                request.RazorpaySignature,
                request.TenantId,
                request.Amount,
                request.Currency
            );

            return Ok(new { success = result.Status == "CAPTURED", data = result });
        }
    }

    // ================================
    // Request Models
    // ================================
    public class RazorpayVerifyRequest
    {
        public string RazorpayOrderId { get; set; } = string.Empty;
        public string RazorpayPaymentId { get; set; } = string.Empty;
        public string RazorpaySignature { get; set; } = string.Empty;
        public int TenantId { get; set; }
        public decimal Amount { get; set; } // send rupees from frontend
        public string Currency { get; set; } = "INR";
    }

    public class CreateOrderRequest
    {
        public int TenantId { get; set; }
        public decimal Amount { get; set; }
    }
}
