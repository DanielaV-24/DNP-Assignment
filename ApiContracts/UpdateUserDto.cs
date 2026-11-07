namespace ApiContracts;

public class UpdateUserDto
{
    public required string Username { get; set; }
    public string? Password { get; set; }
}