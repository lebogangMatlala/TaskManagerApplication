using Microsoft.EntityFrameworkCore;
using TaskManagerApplication.Data;
using TaskManagerApplication.DTOs;
using TaskManagerApplication.Models;
using TaskManagerApplication.Services;

namespace TaskManagerApplication.ServicesImpl
{
    public class TaskService : ITaskService
    {
        private readonly AppDbContext _context;

        public TaskService(AppDbContext context)
        {
            _context = context;
        }

        private TaskResponseDto MapToDto(TaskItem task) => new TaskResponseDto
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            Priority = task.Priority,
            Status = task.Status,
            DueDate = task.DueDate,
            ProjectId = task.ProjectId,
            ProjectName = task.Project?.Name
        };

        public async Task<TaskResponseDto> CreateTaskAsync(int projectId, CreateTaskDto dto, int userId)
        {
            var project = await _context.Projects
                .FirstOrDefaultAsync(p => p.Id == projectId && p.UserId == userId);

            if (project == null)
                throw new KeyNotFoundException($"Project not found or does not belong to user {userId}.");

            var task = new TaskItem
            {
                Title = dto.Title,
                Description = dto.Description,
                Priority = dto.Priority,
                Status = dto.Status,
                DueDate = dto.DueDate,
                ProjectId = projectId,
                Project = project
            };

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            return MapToDto(task);
        }

        public async Task<(List<TaskResponseDto> Tasks, int TotalCount)> GetTasksAsync(
            int projectId,
            int userId,
            string search = null,
            Status? status = null,
            TaskPriority? priority = null,
            int page = 1,
            int pageSize = 10)
        {
            var query = _context.Tasks
                .Include(t => t.Project)
                .Where(t => t.ProjectId == projectId && t.Project.UserId == userId)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(t => t.Title.Contains(search) || t.Description.Contains(search));

            if (status.HasValue)
                query = query.Where(t => t.Status == status.Value);

            if (priority.HasValue)
                query = query.Where(t => t.Priority == priority.Value);

            var totalCount = await query.CountAsync();

            var tasks = await query
                .OrderBy(t => t.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (tasks.Select(MapToDto).ToList(), totalCount);
        }

        public async Task<TaskResponseDto> GetTaskByIdAsync(int projectId, int taskId, int userId)
        {
            var task = await _context.Tasks
                .Include(t => t.Project)
                .FirstOrDefaultAsync(t => t.Id == taskId && t.ProjectId == projectId && t.Project.UserId == userId);

            if (task == null)
                throw new KeyNotFoundException("Task not found or access denied.");

            return MapToDto(task);
        }

        public async Task<TaskResponseDto> UpdateTaskAsync(int projectId, int taskId, UpdateTaskDto dto, int userId)
        {
            var task = await _context.Tasks
                .Include(t => t.Project)
                .FirstOrDefaultAsync(t => t.Id == taskId && t.ProjectId == projectId && t.Project.UserId == userId);

            if (task == null)
                throw new KeyNotFoundException("Task not found or access denied.");

            task.Title = dto.Title ?? task.Title;
            task.Description = dto.Description ?? task.Description;
            task.Priority = dto.Priority ?? task.Priority;
            task.Status = dto.Status ?? task.Status;
            task.DueDate = dto.DueDate ?? task.DueDate;

            _context.Tasks.Update(task);
            await _context.SaveChangesAsync();

            return MapToDto(task);
        }

        public async Task<bool> DeleteTaskAsync(int projectId, int taskId, int userId)
        {
            var task = await _context.Tasks
                .Include(t => t.Project)
                .FirstOrDefaultAsync(t => t.Id == taskId && t.ProjectId == projectId && t.Project.UserId == userId);

            if (task == null) return false;

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}