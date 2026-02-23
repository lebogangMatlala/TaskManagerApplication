using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManagerApplication.DTOs;
using TaskManagerApplication.Services;

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
            return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new Exception("User ID not found in token"));
        }

 
        [HttpPost("CreateProject")]
        public async Task<IActionResult> Create([FromBody] CreateProjectDto dto)
        {
            var userId = GetUserId();
            var project = await _projectService.CreateProjectAsync(userId, dto);
            return Ok(project);
        }
        
        [HttpGet("GetAllProjects")]
        public async Task<IActionResult> GetAll()
        {
            var userId = GetUserId();
            var projects = await _projectService.GetProjectsAsync(userId);
            return Ok(projects);
        }

        [HttpGet("GetProject{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var userId = GetUserId();
            var project = await _projectService.GetProjectByIdAsync(userId, id);
            return Ok(project);
        }

        [HttpPut("UpdateProject{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateProjectDto dto)
        {
            var userId = GetUserId();
            var project = await _projectService.UpdateProjectAsync(userId, id, dto);
            return Ok(project);
        }

        [HttpDelete("DeleteProject{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = GetUserId();
            await _projectService.DeleteProjectAsync(userId, id);
            return NoContent();
        }
    }
}
