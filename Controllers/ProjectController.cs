using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using TaskManagerApplication.DTOs;
using TaskManagerApplication.Models;
using TaskManagerApplication.Services;
using TaskManagerApplication.ServicesImpl;

namespace TaskManagerApplication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // JWT Protected
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        private int GetUserId()
        {
            var userIdClaim =
                User.FindFirstValue(ClaimTypes.NameIdentifier) ??
                User.FindFirstValue("sub");

            if (string.IsNullOrEmpty(userIdClaim))
                throw new Exception("User ID not found in token");

            return int.Parse(userIdClaim);
        }



        [HttpPost("CreateProject")]
        public async Task<IActionResult> Create([FromBody] CreateProjectDto dto)
        {
            var userId = GetUserId();
            var project = await _projectService.CreateProjectAsync(userId, dto);
            return Ok(project);
        }

        [HttpGet("GetAllProjects")]
        public async Task<IActionResult> GetAll(
     [FromQuery] string search = null,
     [FromQuery] ProjectStatus? status = null,
     [FromQuery] int page = 1,
     [FromQuery] int pageSize = 10)
        {
            var userId = GetUserId();
            var (projects, totalCount) = await _projectService.GetProjectsAsync(userId, search, status, page, pageSize);

            return Ok(new
            {
                Projects = projects,
                TotalCount = totalCount
            });
        }

        [HttpGet("GetProject/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var userId = GetUserId();
            var project = await _projectService.GetProjectByIdAsync(userId, id);
            return Ok(project);
        }

        [HttpPut("UpdateProject/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateProjectDto dto)
        {
            var userId = GetUserId();
            var project = await _projectService.UpdateProjectAsync(userId, id, dto);
            return Ok(project);
        }

        [HttpDelete("DeleteProject/{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            var userId = GetUserId();

            // Attempt to delete the project for the current user
            var deleted = await _projectService.DeleteProjectAsync(userId, id);

            if (!deleted)
                return NotFound(new { Message = "Project not found or access denied." });

            // Return a success message
            return Ok(new { Message = $"Project with ID {id} successfully deleted." });
        }


    }
}
