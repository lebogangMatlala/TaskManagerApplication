namespace TaskManagerApplication.DTOs;

public class UserResponseDto
{
    public int Id { get; set; }
    public string Email { get; set; }

    public string Name { get; set; }
    public string Surname { get; set; }

    // optional convenience property
    public string FullName { get; set; }
    public string Token { get; set; } // JWT Token
}
