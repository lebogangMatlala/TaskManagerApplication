using TaskManagerApplication.DTOs;
using TaskManagerApplication.Models;

namespace TaskManagerApplication.Services;

public interface IAuthService
{
    Task<UserResponseDto> RegisterAsync(RegisterDto dto);
    Task<UserResponseDto> LoginAsync(LoginDto dto);
}
