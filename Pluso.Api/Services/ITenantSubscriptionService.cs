using Pluso.Api.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pluso.Api.Services
{
    public interface ITenantSubscriptionService
    {
        Task<IEnumerable<TenantSubscription>> GetAllAsync();
        Task<TenantSubscription> GetByIdAsync(int id);
        Task<IEnumerable<TenantSubscription>> GetByTenantIdAsync(int tenantId);
        Task<int> CreateAsync(TenantSubscription subscription);
        Task<int> UpdateAsync(TenantSubscription subscription);
        Task<int> UpdatePartialAsync(TenantSubscription subscription);

        Task<bool> DeleteAsync(int id);
    }
}
