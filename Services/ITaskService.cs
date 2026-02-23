using TaskManagerApplication.DTOs;
using TaskManagerApplication.Models;

namespace TaskManagerApplication.Services
{
    public interface ITaskService
    {
        Task<TaskResponseDto> CreateTaskAsync(int projectId, CreateTaskDto dto);

        Task<(List<TaskResponseDto> Tasks, int TotalCount)> GetTasksAsync(
            int projectId,
            string search = null,
            Status? status = null,
            TaskPriority? priority = null,
            int page = 1,
            int pageSize = 10);

        Task<TaskResponseDto> GetTaskByIdAsync(int projectId, int taskId);

        Task<TaskResponseDto> UpdateTaskAsync(int projectId, int taskId, UpdateTaskDto dto);

        Task<bool> DeleteTaskAsync(int projectId, int taskId);
    }
}
