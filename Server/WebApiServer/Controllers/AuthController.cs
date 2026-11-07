using ApiContracts;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebApiServer.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserRepository userRepository;

    public AuthController(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        // Find user by username
        var user = userRepository.GetMany()
            .FirstOrDefault(u => u.Username == request.Username);

        // Check if user exists
        if (user == null)
        {
            return Unauthorized();
        }

        // Check password
        if (user.Password != request.Password)
        {
            return Unauthorized();
        }

        // Convert User to UserDto
        var userDto = new UserDto
        {
            Id = user.Id,
            Username = user.Username
        };

        return Ok(userDto);
    }
}