using Microsoft.AspNetCore.Mvc;
using Pluso.Api.Model;
using Pluso.Api.Services;
using System.Threading.Tasks;

namespace Pluso.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TenantSubscriptionController : ControllerBase
    {
        private readonly ITenantSubscriptionService _service;

        public TenantSubscriptionController(ITenantSubscriptionService service)
        {
            _service = service;
        }

        // GET: api/TenantSubscription
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        // GET: api/TenantSubscription/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null)
                return NotFound(new { Message = $"TenantSubscription with Id {id} not found." });

            return Ok(result);
        }

        // GET: api/TenantSubscription/Tenant/{tenantId}
        [HttpGet("Tenant/{tenantId}")]
        public async Task<IActionResult> GetByTenantId(int tenantId)
        {
            var result = await _service.GetByTenantIdAsync(tenantId);
            return Ok(result);
        }

        // POST: api/TenantSubscription
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TenantSubscription subscription)
        {
            if (subscription == null)
                return BadRequest(new { Message = "Invalid subscription data." });

            var id = await _service.CreateAsync(subscription);
            return Ok(new { Message = "TenantSubscription created successfully.", Id = id });
        }

        // PUT: api/TenantSubscription/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] TenantSubscription subscription)
        {
            if (subscription == null || subscription.TenantSubscriptionId != id)
                return BadRequest(new { Message = "Invalid subscription data." });

            var updated = await _service.UpdateAsync(subscription);
            if (updated <= 0)
                return NotFound(new { Message = $"TenantSubscription with Id {id} not found or update failed." });

            return Ok(new { Message = "TenantSubscription updated successfully." });
        }

        [HttpPatch("UpdatePartial")]
        public async Task<IActionResult> UpdatePartial([FromBody] TenantSubscription model)
        {
            var result = await _service.UpdatePartialAsync(model);

            return result > 0
                ? Ok(new { message = "Subscription updated successfully" })
                : BadRequest("Update failed");
        }


        // DELETE: api/TenantSubscription/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted)
                return NotFound(new { Message = $"TenantSubscription with Id {id} not found or delete failed." });

            return Ok(new { Message = "TenantSubscription deleted successfully." });
        }
    }
}
