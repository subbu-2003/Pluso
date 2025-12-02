using Pluso.Api.Model;

namespace Pluso.Api.Repositorys
{
    public interface IRazorpay_VerificationRepository
    {
        Task<int> AddVerificationAsync(Razorpay_Verification verification);
        Task<IEnumerable<Razorpay_Verification>> GetAllVerificationsAsync();
        Task<Razorpay_Verification?> GetVerificationByPaymentIdAsync(string razorpayPaymentId);
    }
}
