namespace Pluso.Api.Model
{
    public class Razorpay_Verification
    {
        public int VerificationId { get; set; }

        public string RazorpayOrderId { get; set; }
        public string? RazorpayPaymentId { get; set; }
        public string? RazorpaySignature { get; set; }

        public int TenantId { get; set; }

        public string Currency { get; set; }
        public decimal Amount { get; set; }

        public string Status { get; set; }   // created, paid, failed, verified, captured

        public DateTime? VerifiedAt { get; set; }
        public DateTime? CapturedAt { get; set; }

        public string? ResponseJson { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
