namespace Pluso.Api.Model
{
    public class Tenant
    {
        public int TenantId { get; set; }
        public string TenantName { get; set; }
        public string TenantSlug { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public bool IsVerifiedEmail { get; set; }
        public bool IsVerifiedPhoneNo { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
    }
}
