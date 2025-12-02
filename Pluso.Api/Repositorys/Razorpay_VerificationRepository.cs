using Dapper;
using ecom.DBcontexts;
using Pluso.Api.Model;

namespace Pluso.Api.Repositorys
{
    public class Razorpay_VerificationRepository : IRazorpay_VerificationRepository
    {

        public readonly IDbContext _dbContext;

        public Razorpay_VerificationRepository(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> AddVerificationAsync(Razorpay_Verification verification)
        {
            string sql = @"
                INSERT INTO Razorpay_Verifications
                (RazorpayOrderId, RazorpayPaymentId, RazorpaySignature, TenantId, Amount, Currency, Status, VerifiedAt, CapturedAt, ResponseJson, CreatedAt)
                VALUES
                (@RazorpayOrderId, @RazorpayPaymentId, @RazorpaySignature, @TenantId, @Amount, @Currency, @Status, @VerifiedAt, @CapturedAt, @ResponseJson, @CreatedAt);
                SELECT LAST_INSERT_ID();";

            using (var conn = _dbContext.GetConnection())
            {
                conn.Open();
                return await conn.ExecuteScalarAsync<int>(sql, verification);
            }
        }

        // ================================
        // 2️⃣ GET ALL VERIFICATIONS
        // ================================
        public async Task<IEnumerable<Razorpay_Verification>> GetAllVerificationsAsync()
        {
            string sql = "SELECT * FROM Razorpay_Verifications ORDER BY CreatedAt DESC;";

            using (var conn = _dbContext.GetConnection())
            {
                conn.Open();
                return await conn.QueryAsync<Razorpay_Verification>(sql);
            }
        }

        // ================================
        // 3️⃣ GET BY PAYMENT ID
        // ================================
        public async Task<Razorpay_Verification?> GetVerificationByPaymentIdAsync(string razorpayPaymentId)
        {
            string sql = "SELECT * FROM Razorpay_Verifications WHERE RazorpayPaymentId = @razorpayPaymentId;";

            using (var conn = _dbContext.GetConnection())
            {
                conn.Open();
                return await conn.QueryFirstOrDefaultAsync<Razorpay_Verification>(sql, new { razorpayPaymentId });
            }
        }
    }
}
