namespace Entities;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
    
    // Navigation properties
    public ICollection<Post> Posts { get; set; } = [];
    public ICollection<Comment> Comments { get; set; } = [];

    private User() { } // For EFC

    
    public User(string userName, string password)
    {
        Username = userName;
        Password = password;
    }
}