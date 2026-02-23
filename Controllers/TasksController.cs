using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        [HttpPost]
        public async Task<ActionResult<TaskResponseDto>> CreateTask(int projectId, [FromBody] CreateTaskDto createTaskDto)
        {
            var taskDto = await _taskService.CreateTaskAsync(projectId, createTaskDto);
            return CreatedAtAction(nameof(GetTaskById), new { projectId, taskId = taskDto.Id }, taskDto);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskResponseDto>>> GetTasks(int projectId)
        {
            var tasks = await _taskService.GetTasksAsync(projectId);
            return Ok(tasks);
        }


        [HttpGet("{taskId}")]
        public async Task<ActionResult<TaskResponseDto>> GetTaskById(int projectId, int taskId)
        {
            var task = await _taskService.GetTaskByIdAsync(projectId, taskId);
            return Ok(task);
        }

        [HttpPut("{taskId}")]
        public async Task<ActionResult<TaskResponseDto>> UpdateTask(int projectId, int taskId, [FromBody] UpdateTaskDto updateTaskDto)
        {
            var task = await _taskService.UpdateTaskAsync(projectId, taskId, updateTaskDto);
            return Ok(task);
        }

        [HttpDelete("{taskId}")]
        public async Task<IActionResult> DeleteTask(int projectId, int taskId)
        {
            var deleted = await _taskService.DeleteTaskAsync(projectId, taskId);
            if (!deleted) return NotFound($"Task with ID {taskId} not found in project {projectId}.");
            return NoContent();
        }
    }
}
