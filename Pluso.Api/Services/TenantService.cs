using Pluso.Api.Model;
using Pluso.Api.Repositorys;

namespace Pluso.Api.Services
{
    public class TenantService : ITenantService
    {
        private readonly ITenantRepository _tenantRepository;

        public TenantService(ITenantRepository tenantRepository)
        {
            _tenantRepository = tenantRepository;
        }

        public Task<IEnumerable<Tenant>> GetAllAsync()
        {
            return _tenantRepository.GetAllAsync();
        }

        public Task<Tenant?> GetByIdAsync(int tenantId)
        {
            return _tenantRepository.GetByIdAsync(tenantId);
        }

        public Task<int> CreateAsync(Tenant tenant)
        {
            tenant.CreatedAt = DateTime.Now;
            tenant.ModifiedAt = DateTime.Now;
            return _tenantRepository.CreateAsync(tenant);
        }

        public Task<int> UpdateAsync(Tenant tenant)
        {
            tenant.ModifiedAt = DateTime.Now;
            return _tenantRepository.UpdateAsync(tenant);
        }

        public Task<int> DeleteAsync(int tenantId)
        {
            return _tenantRepository.DeleteAsync(tenantId);
        }
    }
}
