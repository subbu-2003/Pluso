using Pluso.Api.Model;

namespace Pluso.Api.Services
{
    public interface ITenantService
    {
        Task<IEnumerable<Tenant>> GetAllAsync();
        Task<Tenant?> GetByIdAsync(int tenantId);
        Task<int> CreateAsync(Tenant tenant);
        Task<int> UpdateAsync(Tenant tenant);
        Task<int> DeleteAsync(int tenantId);
    }
}
