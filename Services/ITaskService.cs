using TaskManagerApplication.DTOs;
using TaskManagerApplication.Models;

namespace TaskManagerApplication.Services
{
    public interface ITaskService
    {
        Task<TaskResponseDto> CreateTaskAsync(int projectId, CreateTaskDto dto, int userId);

        Task<(List<TaskResponseDto> Tasks, int TotalCount)> GetTasksAsync(
            int projectId,
            int userId,
            string search = null,
            Status? status = null,
            TaskPriority? priority = null,
            int page = 1,
            int pageSize = 10);

        Task<TaskResponseDto> GetTaskByIdAsync(int projectId, int taskId, int userId);

        Task<TaskResponseDto> UpdateTaskAsync(int projectId, int taskId, UpdateTaskDto dto, int userId);

        Task<bool> DeleteTaskAsync(int projectId, int taskId, int userId);
    }
}