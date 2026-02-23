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

        public async Task<TaskResponseDto> CreateTaskAsync(int projectId, CreateTaskDto createTaskDto)
        {
            var project = await _context.Projects.FindAsync(projectId);
            if (project == null)
                throw new KeyNotFoundException($"Project with ID {projectId} not found.");

            var task = new TaskItem
            {
                Title = createTaskDto.Title,
                Description = createTaskDto.Description,
                Priority = createTaskDto.Priority,
                Status = createTaskDto.Status,
                DueDate = createTaskDto.DueDate,
                ProjectId = projectId,
                Project = project
            };

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            return MapToDto(task);
        }

        public async Task<(List<TaskResponseDto> Tasks, int TotalCount)> GetTasksAsync(
     int projectId,
     string search = null,
     Status? status = null,
     TaskPriority? priority = null,
     int page = 1,
     int pageSize = 10)
        {
            var query = _context.Tasks
                .Where(t => t.ProjectId == projectId)
                .Include(t => t.Project)
                .AsQueryable();

            // Text search
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(t => t.Title.Contains(search) || t.Description.Contains(search));
            }

            // Status filter
            if (status.HasValue)
            {
                query = query.Where(t => t.Status == status.Value);
            }

            // Priority filter
            if (priority.HasValue)
            {
                query = query.Where(t => t.Priority == priority.Value);
            }

            var totalCount = await query.CountAsync();

            var tasks = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (tasks.Select(MapToDto).ToList(), totalCount);
        }


        public async Task<TaskResponseDto> GetTaskByIdAsync(int projectId, int taskId)
        {
            var task = await _context.Tasks
                .Include(t => t.Project)
                .FirstOrDefaultAsync(t => t.ProjectId == projectId && t.Id == taskId);

            if (task == null)
                throw new KeyNotFoundException($"Task with ID {taskId} not found in project {projectId}.");

            return MapToDto(task);
        }

        public async Task<TaskResponseDto> UpdateTaskAsync(int projectId, int taskId, UpdateTaskDto dto)
        {
            var task = await _context.Tasks
                .Include(t => t.Project)
                .FirstOrDefaultAsync(t => t.ProjectId == projectId && t.Id == taskId);

            if (task == null)
                throw new KeyNotFoundException($"Task with ID {taskId} not found in project {projectId}.");

            task.Title = dto.Title ?? task.Title;
            task.Description = dto.Description ?? task.Description;

            if (dto.Priority.HasValue)
                task.Priority = dto.Priority.Value;

            if (dto.Status.HasValue)
                task.Status = dto.Status.Value;

            task.DueDate = dto.DueDate ?? task.DueDate;

            _context.Tasks.Update(task);
            await _context.SaveChangesAsync();

            return MapToDto(task);
        }

        public async Task<bool> DeleteTaskAsync(int projectId, int taskId)
        {
            var task = await _context.Tasks.FirstOrDefaultAsync(t => t.ProjectId == projectId && t.Id == taskId);
            if (task == null) return false;

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
