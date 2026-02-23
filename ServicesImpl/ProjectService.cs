using Microsoft.EntityFrameworkCore;
using TaskManagerApplication.Data;
using TaskManagerApplication.DTOs;
using TaskManagerApplication.Models;
using TaskManagerApplication.Services;

namespace TaskManagerApplication.ServicesImpl
{
    public class ProjectService : IProjectService
    {
        private readonly AppDbContext _context;

        public ProjectService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ProjectResponseDto> CreateProjectAsync(int userId, CreateProjectDto dto)
        {
            var project = new Project
            {
                Name = dto.Name,
                Description = dto.Description,
                Status = dto.Status ?? ProjectStatus.Active, // <- replaced string with enum
                UserId = userId
            };

            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            return new ProjectResponseDto
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                Status = project.Status // enum will serialize as string if configured in DbContext
            };
        }

        public async Task<(List<ProjectResponseDto> Projects, int TotalCount)> GetProjectsAsync(
    int userId,
    string search = null,
    ProjectStatus? status = null,
    int page = 1,
    int pageSize = 10)
{
    var query = _context.Projects
        .Where(p => p.UserId == userId)
        .AsQueryable();

    // Text search
    if (!string.IsNullOrWhiteSpace(search))
    {
        query = query.Where(p => p.Name.Contains(search) || p.Description.Contains(search));
    }

    // Status filter
    if (status.HasValue)
    {
        query = query.Where(p => p.Status == status.Value);
    }

    var totalCount = await query.CountAsync();

    var projects = await query
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .Select(p => new ProjectResponseDto
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            Status = p.Status
        })
        .ToListAsync();

    return (projects, totalCount);
}


        public async Task<ProjectResponseDto> GetProjectByIdAsync(int userId, int projectId)
        {
            var project = await _context.Projects
                .FirstOrDefaultAsync(p => p.Id == projectId && p.UserId == userId);

            if (project == null)
                throw new KeyNotFoundException("Project not found");

            return new ProjectResponseDto
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                Status = project.Status // enum
            };
        }

        public async Task<ProjectResponseDto> UpdateProjectAsync(int userId, int projectId, UpdateProjectDto dto)
        {
            var project = await _context.Projects
                .FirstOrDefaultAsync(p => p.Id == projectId && p.UserId == userId);

            if (project == null)
                throw new KeyNotFoundException("Project not found");

            project.Name = dto.Name ?? project.Name;
            project.Description = dto.Description ?? project.Description;

            // Update enum safely
            if (dto.Status.HasValue)
                project.Status = dto.Status.Value;

            await _context.SaveChangesAsync();

            return new ProjectResponseDto
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                Status = project.Status
            };
        }

        public async Task DeleteProjectAsync(int userId, int projectId)
        {
            var project = await _context.Projects
                .Include(p => p.Tasks)
                .FirstOrDefaultAsync(p => p.Id == projectId && p.UserId == userId);

            if (project == null)
                throw new KeyNotFoundException("Project not found");

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
        }
    }
}
