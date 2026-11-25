using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TenantHub.Dtos;
using TenantHub.Services;

namespace TenantHub.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Roles = "User")]
    public class ProjectsController : ControllerBase
    {
        private readonly ProjectService _projectService;

        public ProjectsController(ProjectService projectService)
        {
            _projectService = projectService;
        }

        private int GetTenantId() => int.Parse(User.FindFirst("TenantId")!.Value);
        private int GetUserId() => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var projects = await _projectService.GetAllAsync(GetTenantId());
            return Ok(projects);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var project = await _projectService.GetByIdAsync(id, GetTenantId());
            if (project == null) return NotFound();
            return Ok(project);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProjectRequest request)
        {
            if (await _projectService.NameExistsAsync(request.Name, GetTenantId(), null))
                return Conflict("A project with this name already exists.");

            var project = await _projectService.CreateAsync(request, GetTenantId(), GetUserId());
            return CreatedAtAction(nameof(GetById), new { id = project.Id }, project);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ProjectRequest request)
        {
            if (await _projectService.GetByIdAsync(id, GetTenantId()) == null)
                return NotFound();

            if (await _projectService.NameExistsAsync(request.Name, GetTenantId(),id))
                return Conflict("A project with this name already exists.");

            var project = await _projectService.UpdateAsync(id, request, GetTenantId());
            return Ok(project);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _projectService.DeleteAsync(id, GetTenantId());
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}