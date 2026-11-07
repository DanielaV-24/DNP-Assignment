using ApiContracts;

namespace BlazorApp.Services;

public interface IUserService
{
    Task<IEnumerable<UserDto>> GetAllUsersAsync(string? username = null);

    Task<UserDto?> GetUserByIdAsync(int id);

    Task<UserDto> AddUserAsync(CreateUserDto request);

    Task UpdateUserAsync(int id, UpdateUserDto request);

    Task DeleteUserAsync(int id);
}