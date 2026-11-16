namespace Entities;

public class Comment
{
    public int Id { get; set; }
    public string Body { get; set; } = null!;
    
    // Foreign keys
    public int UserId { get; set; }
    public int PostId { get; set; }

    // Navigation properties
    public User User { get; set; } = null!;
    public Post Post { get; set; } = null!;

    private Comment() { } // For EFC
    
    public Comment(string body, int postId, int userId)
    {
        Body = body;
        PostId = postId;
        UserId = userId;
    }
}