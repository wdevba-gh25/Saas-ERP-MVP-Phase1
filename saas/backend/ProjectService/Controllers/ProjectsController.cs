using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared;
using Shared.Entities;

namespace ProjectService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectsController : ControllerBase
    {
        private readonly SaaSMvpContext _db;
        public ProjectsController(SaaSMvpContext db) => _db = db;

        [Authorize]
        [HttpGet("my-projects")]
        public async Task<IActionResult> GetMyProjects()
        {
            var orgIdClaim = User.FindFirst("organizationId")?.Value;

            if (string.IsNullOrEmpty(orgIdClaim) || !Guid.TryParse(orgIdClaim, out var organizationId))
                return Unauthorized("Organization context is missing or invalid.");

            var projects = await _db.Projects
                .Where(p => p.OrganizationId == organizationId)
                .ToListAsync();


            return Ok(projects);
        }
    }
}
