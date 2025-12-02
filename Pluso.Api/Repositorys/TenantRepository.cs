using Dapper;
using ecom.DBcontexts;
using Pluso.Api.Model;

namespace Pluso.Api.Repositorys
{
    public class TenantRepository : ITenantRepository
    {
        private readonly IDbContext _dbContext;

        public TenantRepository(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Tenant>> GetAllAsync()
        {
            using var conn = _dbContext.GetConnection();
            return await conn.QueryAsync<Tenant>("SELECT * FROM tenant");
        }

        public async Task<Tenant?> GetByIdAsync(int tenantId)
        {
            using var conn = _dbContext.GetConnection();
            return await conn.QueryFirstOrDefaultAsync<Tenant>(
                "SELECT * FROM tenant WHERE TenantId=@TenantId",
                new { TenantId = tenantId });
        }

        public async Task<int> CreateAsync(Tenant tenant)
        {
            var sql = @"
        INSERT INTO tenant
        (TenantName, TenantSlug, TenantDbName, Email, MobileNumber, Location, IsActive, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy)
        VALUES
        (@TenantName, @TenantSlug, @TenantDbName, @Email, @MobileNumber, @Location, @IsActive, @CreatedAt, @ModifiedAt, @CreatedBy, @ModifiedBy);
        SELECT LAST_INSERT_ID();";

            using var conn = _dbContext.GetConnection();
            return await conn.ExecuteScalarAsync<int>(sql, tenant);
        }


        public async Task<int> UpdateAsync(Tenant tenant)
        {
            var sql = @"
        UPDATE tenant SET
            TenantName = @TenantName,
            TenantSlug = @TenantSlug,
            TenantDbName = @TenantDbName,
            Email = @Email,
            MobileNumber = @MobileNumber,
            Location = @Location,
            IsActive = @IsActive,
            ModifiedAt = @ModifiedAt,
            ModifiedBy = @ModifiedBy
        WHERE TanantId = @TanantId";

            using var conn = _dbContext.GetConnection();
            return await conn.ExecuteAsync(sql, tenant);
        }

        public async Task<int> DeleteAsync(int tenantId)
        {
            using var conn = _dbContext.GetConnection();
            return await conn.ExecuteAsync(
                "DELETE FROM tenant WHERE TenantId=@TenantId",
                new { TenantId = tenantId });
        }
    }
}
