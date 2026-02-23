namespace TaskManagerApplication.DTOs;

public class UserResponseDto
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string Token { get; set; } // JWT Token
}
