using Pluso.Api.Model;
using Pluso.Api.Repositorys;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pluso.Api.Services
{
    public class TenantSubscriptionService : ITenantSubscriptionService
    {
        private readonly ITenantSubscriptionRepository _repository;

        public TenantSubscriptionService(ITenantSubscriptionRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<TenantSubscription>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<TenantSubscription> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<TenantSubscription>> GetByTenantIdAsync(int tenantId)
        {
            return await _repository.GetByTenantIdAsync(tenantId);
        }

        public async Task<int> CreateAsync(TenantSubscription subscription)
        {
            // Set CreatedAt / ModifiedAt
            subscription.CreatedAt = DateTime.Now;
            subscription.ModifiedAt = DateTime.Now;

            // Default IsActiveSubscription = 0 if not set
            subscription.IsActiveSubscription = false;
            subscription.PaymentStatus = "Failed";

            // Calculate EndDate based on SubscriptionType
            if (!string.IsNullOrWhiteSpace(subscription.SubscriptionType) && subscription.StartDate != null)
            {
                switch (subscription.SubscriptionType.ToLower())
                {
                    case "month":
                    case "monthly":
                        subscription.EndDate = subscription.StartDate.AddMonths(1);
                        break;

                    case "year":
                    case "yearly":
                        subscription.EndDate = subscription.StartDate.AddYears(1);
                        break;

                    default:
                        subscription.EndDate = subscription.StartDate.AddMonths(1);
                        break;
                }
            }

            // Handle trial only if IsTrial = true
            if (subscription.IsTrial)
            {
                subscription.TrialStartDate = subscription.EndDate.AddDays(7); // Trial starts 1 week after subscription ends
                subscription.TrialEndDate = subscription.TrialStartDate.Value.AddDays(7); // Trial lasts 1 week
            }
            else
            {
                subscription.TrialStartDate = null;
                subscription.TrialEndDate = null;
            }

            // Insert into database
            return await _repository.CreateAsync(subscription);
        }

        public async Task<int> UpdatePartialAsync(TenantSubscription subscription)
        {
            subscription.ModifiedAt = DateTime.Now;

            // Auto calculate End Date
            if (!string.IsNullOrEmpty(subscription.SubscriptionType))
            {
                switch (subscription.SubscriptionType.ToLower())
                {
                    case "month":
                    case "monthly":
                        subscription.EndDate = subscription.StartDate.AddMonths(1);
                        break;

                    case "year":
                    case "yearly":
                        subscription.EndDate = subscription.StartDate.AddYears(1);
                        break;
                }
            }

            return await _repository.UpdatePartialAsync(subscription);
        }




        public async Task<int> UpdateAsync(TenantSubscription subscription)
        {
            subscription.ModifiedAt = System.DateTime.Now;
            return await _repository.UpdateAsync(subscription);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }
    }
}
