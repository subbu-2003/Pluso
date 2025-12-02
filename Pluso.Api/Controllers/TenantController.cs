using Microsoft.AspNetCore.Mvc;
using Pluso.Api.Model;
using Pluso.Api.Services;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Pluso.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TenantController : ControllerBase
    {
        private readonly ITenantService _tenantService;

        public TenantController(ITenantService tenantService)
        {
            _tenantService = tenantService;
        }

        // GET: api/tenant
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tenant>>> GetAll()
        {
            var tenants = await _tenantService.GetAllAsync();
            return Ok(tenants);
        }

        // GET: api/tenant/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Tenant>> GetById(int id)
        {
            var tenant = await _tenantService.GetByIdAsync(id);
            if (tenant == null) return NotFound(new { Message = "Tenant not found." });
            return Ok(tenant);
        }

        // POST: api/tenant
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Tenant tenant)
        {
            if (tenant == null)
                return BadRequest(new { Message = "Invalid tenant data." });

            tenant.CreatedAt = System.DateTime.Now;
            tenant.ModifiedAt = System.DateTime.Now;

            var id = await _tenantService.CreateAsync(tenant);
            tenant.TenantId = id;

            return CreatedAtAction(nameof(GetById), new { id = tenant.TenantId }, tenant);
        }

        // PUT: api/tenant/Update/{id}
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Tenant tenant)
        {
            if (tenant == null || id <= 0)
                return BadRequest(new { Message = "Invalid tenant data." });

            tenant.TenantId = id;
            tenant.ModifiedAt = System.DateTime.Now;

            var updatedTenant = await _tenantService.UpdateAsync(tenant); // make UpdateAsync return Tenant or null
            return updatedTenant != null
                ? Ok(new { message = "Tenant Updated Successfully", data = updatedTenant })
                : BadRequest("Update Failed");
        }

        // DELETE: api/tenant/Delete/{id}
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
                return BadRequest(new { Message = "Invalid tenant id." });

            var deleted = await _tenantService.DeleteAsync(id); // returns bool
            return Ok(deleted);
        }
    }
}
