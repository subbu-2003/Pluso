using System;

namespace Pluso.Api.Model
{
    public class Tenant
    {
        public int TenantId { get; set; }          // Primary Key (auto-increment)
        public string TenantName { get; set; }     // varchar(150)
        public string TenantSlug { get; set; }     // varchar(150)
        public string TenantDbName { get; set; }   // varchar(150)
        public string Email { get; set; }          // varchar(150)
        public string MobileNumber { get; set; }   // varchar(20)
        public string Location { get; set; }       // varchar(255)
        public bool IsActive { get; set; }         // tinyint(1)
        public DateTime CreatedAt { get; set; }    // datetime
        public DateTime ModifiedAt { get; set; }   // datetime
        public string CreatedBy { get; set; }      // varchar(150)
        public string ModifiedBy { get; set; }     // varchar(150)
    }
}
