using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManagerApplication.DTOs;
using TaskManagerApplication.Services;

namespace TaskManagerApplication.Controllers
{
    [Route("api/projects/{projectId}/tasks")]
    [ApiController]
    [Authorize]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        private int GetUserId()
        {
            var claim = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
            if (string.IsNullOrEmpty(claim)) throw new Exception("User ID not found in token");
            return int.Parse(claim);
        }

        [HttpPost]
        public async Task<ActionResult<TaskResponseDto>> CreateTask(int projectId, [FromBody] CreateTaskDto dto)
        {
            var userId = GetUserId();
            var task = await _taskService.CreateTaskAsync(projectId, dto, userId);
            return CreatedAtAction(nameof(GetTaskById), new { projectId, taskId = task.Id }, task);
        }

        [HttpGet]
        public async Task<IActionResult> GetTasks(int projectId)
        {
            var userId = GetUserId();
            var (tasks, totalCount) = await _taskService.GetTasksAsync(projectId, userId);
            return Ok(new { Tasks = tasks, TotalCount = totalCount });
        }

        [HttpGet("{taskId}")]
        public async Task<ActionResult<TaskResponseDto>> GetTaskById(int projectId, int taskId)
        {
            var userId = GetUserId();
            var task = await _taskService.GetTaskByIdAsync(projectId, taskId, userId);
            return Ok(task);
        }

        [HttpPut("{taskId}")]
        public async Task<ActionResult<TaskResponseDto>> UpdateTask(int projectId, int taskId, [FromBody] UpdateTaskDto dto)
        {
            var userId = GetUserId();
            var task = await _taskService.UpdateTaskAsync(projectId, taskId, dto, userId);
            return Ok(task);
        }

        [HttpDelete("{taskId}")]
        public async Task<IActionResult> DeleteTask(int projectId, int taskId)
        {
            var userId = GetUserId();
            var deleted = await _taskService.DeleteTaskAsync(projectId, taskId, userId);

            if (!deleted)
                return NotFound(new { Message = "Task not found or access denied." });

            return Ok(new { Message = $"Task with ID {taskId} successfully deleted." });
        }
    }
}