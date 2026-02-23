using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskManagerApplication.Data;
using TaskManagerApplication.DTOs;
using TaskManagerApplication.Models;
using TaskManagerApplication.Services;

namespace TaskManagerApplication.ServicesImpl;

public class AuthService : IAuthService
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthService(AppDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<UserResponseDto> RegisterAsync(RegisterDto dto)
    {
        // 1️⃣ Validate input
        if (string.IsNullOrWhiteSpace(dto.Email))
            throw new ArgumentException("Email is required.");

        if (string.IsNullOrWhiteSpace(dto.Password))
            throw new ArgumentException("Password is required.");

        // 2️⃣ Check if user already exists
        if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
            throw new ArgumentException("User already exists.");

        // 3️⃣ Create user with hashed password
        var user = new User
        {
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync(); // Id is now generated

        // 4️⃣ Generate JWT
        var token = GenerateJwtToken(user);

        Console.WriteLine($"[DEBUG] JWT Token for {user.Email}: {token}");
        // 5️⃣ Return response DTO
        return new UserResponseDto
        {
            Id = user.Id,
            Email = user.Email,
            Token = token
        };
    }

    public async Task<UserResponseDto> LoginAsync(LoginDto dto)
    {
        // 1️⃣ Fetch user by email
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);

        // 2️⃣ Verify user exists and password is correct
        if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            throw new ArgumentException("Invalid credentials.");

        // 3️⃣ Generate JWT
        var token = GenerateJwtToken(user);

        // 4️⃣ Return response DTO
        return new UserResponseDto
        {
            Id = user.Id,
            Email = user.Email,
            Token = token
        };
    }

    private string GenerateJwtToken(User user)
    {
        if (user == null) throw new ArgumentNullException(nameof(user));
        if (user.Id == 0) throw new ArgumentException("User Id is not generated yet.");
        if (string.IsNullOrEmpty(user.Email)) throw new ArgumentException("User email is null or empty.");

        var jwtSettings = _configuration.GetSection("Jwt");
        var secret = jwtSettings["Key"];
        var issuer = jwtSettings["Issuer"];
        var audience = jwtSettings["Audience"];
        var expirationMinutes = int.Parse(jwtSettings["ExpirationMinutes"]);

        var claims = new[]
        {
        new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
        new Claim(JwtRegisteredClaimNames.Email, user.Email)
    };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer,
            audience,
            claims,
            expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
            signingCredentials: creds
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

#if DEBUG
        // Print token for debugging only
        Console.WriteLine($"[DEBUG] JWT Token for {user.Email}: {tokenString}");
#endif

        return tokenString;
    }

}
