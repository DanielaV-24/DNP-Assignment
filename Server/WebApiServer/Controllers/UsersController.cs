using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebApiServer.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository userRepository;

    public UsersController(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    // GET: /Users
    // Optional filter by username
    [HttpGet]
    public IActionResult GetAllUsers([FromQuery] string? username)
    {
        var users = userRepository.GetMany();

        if (!string.IsNullOrWhiteSpace(username))
        {
            users = users.Where(u =>
                u.Username.Contains(
                    username,
                    StringComparison.OrdinalIgnoreCase));
        }

        var userDtos = users.Select(user => new UserDto
        {
            Id = user.Id,
            Username = user.Username
        }).ToList();

        return Ok(userDtos);
    }

    // GET: /Users/{id}
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetUserById(int id)
    {
        var user = await userRepository.GetSingleAsync(id);

        if (user is null)
        {
            return NotFound("User not found");
        }

        var userDto = new UserDto
        {
            Id = user.Id,
            Username = user.Username
        };

        return Ok(userDto);
    }

    // POST: /Users
    [HttpPost]
    public async Task<IActionResult> CreateUser(
        [FromBody] CreateUserDto request)
    {
        User newUser = new(
            request.Username,
            request.Password);

        var createdUser = await userRepository.AddAsync(newUser);

        var userDto = new UserDto
        {
            Id = createdUser.Id,
            Username = createdUser.Username
        };

        return CreatedAtAction(
            nameof(GetUserById),
            new { id = createdUser.Id },
            userDto);
    }

    // PUT: /Users/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateUser(
        [FromRoute] int id,
        [FromBody] UpdateUserDto request)
    {
        var user = await userRepository.GetSingleAsync(id);

        if (user is null)
        {
            return NotFound("User not found");
        }

        user.Username = request.Username;

        if (!string.IsNullOrWhiteSpace(request.Password))
        {
            user.Password = request.Password;
        }

        await userRepository.UpdateAsync(user);

        var userDto = new UserDto
        {
            Id = user.Id,
            Username = user.Username
        };

        return Ok(userDto);
    }

    // DELETE: /Users/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var user = await userRepository.GetSingleAsync(id);

        if (user is null)
        {
            return NotFound("User not found");
        }

        await userRepository.DeleteAsync(id);

        return NoContent();
    }
}
