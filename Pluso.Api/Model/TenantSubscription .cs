using System;

namespace Pluso.Api.Model
{
    public class TenantSubscription
    {
        public int TenantSubscriptionId { get; set; }   // Primary Key
        public int TenantId { get; set; }               // Foreign Key to Tenant
        public int PlanId { get; set; }                 // Foreign Key to Plan (if any)
        public string SubscriptionType { get; set; }   // e.g., "Monthly", "Yearly"
        public decimal AmountPaid { get; set; }         // Payment amount
        public int CurrencyId { get; set; }             // Currency identifier
        public DateTime StartDate { get; set; }         // Subscription start date
        public DateTime EndDate { get; set; }           // Subscription end date
        public string PaymentStatus { get; set; }       // e.g., "Paid", "Pending"
        public bool IsTrial { get; set; }               // Trial flag
        public DateTime? TrialStartDate { get; set; }   // Nullable trial start date
        public DateTime? TrialEndDate { get; set; }     // Nullable trial end date
        public bool IsActiveSubscription { get; set; }  // Active subscription flag
        public DateTime CreatedAt { get; set; }         // Record creation
        public DateTime ModifiedAt { get; set; }        // Record modification
        public string CreatedBy { get; set; }           // Creator username/id
        public string ModifiedBy { get; set; }          // Modifier username/id
    }
}
