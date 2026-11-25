using Microsoft.EntityFrameworkCore;
using TenantHub.Data;
using TenantHub.Dtos;
using TenantHub.Models;
namespace TenantHub.Services
{
    public class ProjectService
    {
        private readonly AppDbContext _context;
        public ProjectService(AppDbContext context)
        {
            _context = context;
        }
        private static ProjectResponse ToResponse(Project project)
        {
            return new ProjectResponse
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                Status = project.Status,
                CreatedDate = project.CreatedDate
            };
        }

        public async Task<List<ProjectResponse>> GetAllAsync(int tenantId)
        {
            return await _context.Projects
                .AsNoTracking()
                .Where(p => p.TenantId == tenantId)
                .Select(p => ToResponse(p))
                .ToListAsync();
        }
        public async Task<ProjectResponse?> GetByIdAsync(int id, int tenantId)
        {
            var project = await _context.Projects
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id && p.TenantId == tenantId);

            if (project == null)
            {
                return null;
            }

            return ToResponse(project);
        }
        public async Task<bool> NameExistsAsync(string name, int tenantId, int? excludeId)
        {
            return await _context.Projects
                .AnyAsync(p => p.TenantId == tenantId
                            && p.Name == name
                            && p.Id != excludeId);
        }
        public async Task<ProjectResponse> CreateAsync(ProjectRequest request, int tenantId, int userId)
        {
            var project = new Project
            {
                Name = request.Name,
                Description = request.Description,
                Status = request.Status,
                TenantId = tenantId,
                CreatedByUserId = userId
            };

            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            return ToResponse(project);
        }
        public async Task<ProjectResponse?> UpdateAsync(int id, ProjectRequest request, int tenantId)
        {
            var project = await _context.Projects
                .FirstOrDefaultAsync(p => p.Id == id && p.TenantId == tenantId);

            if (project == null)
            {
                return null;
            }

            project.Name = request.Name;
            project.Description = request.Description;
            project.Status = request.Status;

            await _context.SaveChangesAsync();

            return ToResponse(project);
        }

        public async Task<bool> DeleteAsync(int id, int tenantId)
        {
            var project = await _context.Projects
                .FirstOrDefaultAsync(p => p.Id == id && p.TenantId == tenantId);

            if (project == null)
            {
                return false;
            }

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();

            return true;

        }
    }
}
