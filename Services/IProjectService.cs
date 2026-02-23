using TaskManagerApplication.DTOs;
using TaskManagerApplication.Models;

namespace TaskManagerApplication.Services
{
    public interface IProjectService
    {
        Task<ProjectResponseDto> CreateProjectAsync(int userId, CreateProjectDto dto);
        Task<(List<ProjectResponseDto> Projects, int TotalCount)> GetProjectsAsync(
           int userId,
           string search = null,
           ProjectStatus? status = null,
           int page = 1,
           int pageSize = 10);
        Task<ProjectResponseDto> GetProjectByIdAsync(int userId, int projectId);
        Task<ProjectResponseDto> UpdateProjectAsync(int userId, int projectId, UpdateProjectDto dto);
        Task<bool> DeleteProjectAsync(int userId, int projectId);
    }
}
