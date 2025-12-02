using ecom.DBcontexts;
using Pluso.Api.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;

namespace Pluso.Api.Repositorys
{
    public class TenantSubscriptionRepository : ITenantSubscriptionRepository
    {
        private readonly IDbContext _dbContext;

        public TenantSubscriptionRepository(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Get all TenantSubscriptions
        public async Task<IEnumerable<TenantSubscription>> GetAllAsync()
        {
            var sql = "SELECT * FROM tenantsubscription ORDER BY TenantSubscriptionId DESC;";
            using var conn = _dbContext.GetConnection();
            return await conn.QueryAsync<TenantSubscription>(sql);
        }

        // Get TenantSubscription by Id
        public async Task<TenantSubscription> GetByIdAsync(int id)
        {
            var sql = "SELECT * FROM tenantsubscription WHERE TenantSubscriptionId = @Id;";
            using var conn = _dbContext.GetConnection();
            return await conn.QueryFirstOrDefaultAsync<TenantSubscription>(sql, new { Id = id });
        }

        // Get TenantSubscriptions by TenantId
        public async Task<IEnumerable<TenantSubscription>> GetByTenantIdAsync(int tenantId)
        {
            var sql = "SELECT * FROM tenantsubscription WHERE TenantId = @TenantId ORDER BY StartDate DESC;";
            using var conn = _dbContext.GetConnection();
            return await conn.QueryAsync<TenantSubscription>(sql, new { TenantId = tenantId });
        }

        // Create new TenantSubscription
        public async Task<int> CreateAsync(TenantSubscription subscription)
        {
            var sql = @"
                INSERT INTO tenantsubscription
                (TenantId, PlanId, SubscriptionType, AmountPaid, CurrencyId, StartDate, EndDate, PaymentStatus, IsTrial,
                 TrialStartDate, TrialEndDate, IsActiveSubscription, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy)
                VALUES
                (@TenantId, @PlanId, @SubscriptionType, @AmountPaid, @CurrencyId, @StartDate, @EndDate, @PaymentStatus, @IsTrial,
                 @TrialStartDate, @TrialEndDate, @IsActiveSubscription, @CreatedAt, @ModifiedAt, @CreatedBy, @ModifiedBy);
                SELECT LAST_INSERT_ID();";

            using var conn = _dbContext.GetConnection();
            return await conn.ExecuteScalarAsync<int>(sql, subscription);
        }

        // Update existing TenantSubscription
        public async Task<int> UpdateAsync(TenantSubscription subscription)
        {
            var sql = @"
                UPDATE tenantsubscription SET
                    TenantId = @TenantId,
                    PlanId = @PlanId,
                    SubscriptionType = @SubscriptionType,
                    AmountPaid = @AmountPaid,
                    CurrencyId = @CurrencyId,
                    StartDate = @StartDate,
                    EndDate = @EndDate,
                    PaymentStatus = @PaymentStatus,
                    IsTrial = @IsTrial,
                    TrialStartDate = @TrialStartDate,
                    TrialEndDate = @TrialEndDate,
                    IsActiveSubscription = @IsActiveSubscription,
                    ModifiedAt = @ModifiedAt,
                    ModifiedBy = @ModifiedBy
                WHERE TenantSubscriptionId = @TenantSubscriptionId;";

            using var conn = _dbContext.GetConnection();
            return await conn.ExecuteAsync(sql, subscription);
        }

        public async Task<int> UpdatePartialAsync(TenantSubscription subscription)
        {
            var sql = @"
        UPDATE tenantsubscription SET
            SubscriptionType = @SubscriptionType,
            StartDate = @StartDate,
            EndDate = @EndDate,
            PaymentStatus = @PaymentStatus,
            IsActiveSubscription = @IsActiveSubscription,
            ModifiedAt = @ModifiedAt
        WHERE TenantSubscriptionId = @TenantSubscriptionId;
    ";

            using var conn = _dbContext.GetConnection();
            return await conn.ExecuteAsync(sql, subscription);
        }


        // Delete TenantSubscription by Id
        public async Task<bool> DeleteAsync(int id)
        {
            var sql = "DELETE FROM tenantsubscription WHERE TenantSubscriptionId = @Id;";
            using var conn = _dbContext.GetConnection();
            var affected = await conn.ExecuteAsync(sql, new { Id = id });
            return affected > 0;
        }
    }
}
