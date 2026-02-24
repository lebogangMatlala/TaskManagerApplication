using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManagerApplication.DTOs;
using TaskManagerApplication.Services;

namespace TaskManagerApplication.Controllers;

[ApiController]
[Route("api/[controller]")]
[AllowAnonymous]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        // No try/catch needed — global middleware handles exceptions
        var user = await _authService.RegisterAsync(dto);
        return Ok(user);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = await _authService.LoginAsync(dto);
        return Ok(user);
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



    // ================= UPDATE PROFILE =================
    [Authorize]
    [HttpPut("profile")]
    public async Task<IActionResult> UpdateProfile(RegisterDto dto)
    {
        var userId = GetUserId();
        var result = await _authService.UpdateProfileAsync(userId, dto);
        return Ok(result);
    }

    // ================= DELETE ACCOUNT =================
    [Authorize]
    [HttpDelete("profile")]
    public async Task<IActionResult> DeleteAccount()
    {
        var userId = GetUserId();
        await _authService.DeleteUserAsync(userId);
        return Ok("Account deleted successfully");
    }
}
