using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebApiServer.Controllers;

[ApiController]
[Route("[controller]")]
public class CommentsController : ControllerBase
{
    private readonly ICommentRepository commentRepository;
    private readonly IUserRepository userRepository;
    private readonly IPostRepository postRepository;
    
    public CommentsController(
        ICommentRepository commentRepository,
        IUserRepository userRepository,
        IPostRepository postRepository)
    {
        this.commentRepository = commentRepository;
        this.userRepository = userRepository;
        this.postRepository = postRepository;
    }
    
    // GET endpoint to retrieve comments with optional filters
    [HttpGet]
    public IActionResult GetAllComments(
        [FromQuery] int? postId,
        [FromQuery] int? userId,
        [FromQuery] string? username)
    {
        var comments = commentRepository.GetMany();

        if (postId.HasValue)
        {
            comments = comments.Where(c => c.PostId == postId.Value);
        }

        if (userId.HasValue)
        {
            comments = comments.Where(c => c.UserId == userId.Value);
        }

        var users = userRepository.GetMany().ToList();
        var posts = postRepository.GetMany().ToList();

        if (!string.IsNullOrWhiteSpace(username))
        {
            var matchingUserIds = users
                .Where(u => u.Username.Contains(
                    username,
                    StringComparison.OrdinalIgnoreCase))
                .Select(u => u.Id)
                .ToList();

            comments = comments.Where(c =>
                matchingUserIds.Contains(c.UserId));
        }

        var result = comments.ToList().Select(comment => new
        {
            comment.Id,
            comment.Body,
            Username = users
                .FirstOrDefault(u => u.Id == comment.UserId)?.Username,
            PostTitle = posts
                .FirstOrDefault(p => p.Id == comment.PostId)?.Title
        });

        return Ok(result);
    }

    // GET endpoint to retrieve a specific comment by ID
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetCommentById(int id)
    {
        var comment = await commentRepository.GetSingleAsync(id);

        if (comment is null)
        {
            return NotFound("Comment not found");
        }

        return Ok(comment);
    }

    // POST endpoint to add a new comment
    [HttpPost]
    public async Task<IActionResult> CreateComment(
        [FromBody] Comment newComment)
    {
        var createdComment = await commentRepository.AddAsync(newComment);

        return CreatedAtAction(
            nameof(GetCommentById),
            new { id = createdComment.Id },
            createdComment);
    }

    // PUT endpoint to update an existing comment
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateComment(
        [FromRoute] int id,
        [FromBody] Comment updatedComment)
    {
        if (id != updatedComment.Id)
        {
            return BadRequest(
                "The ID in the URL does not match the comment ID.");
        }

        await commentRepository.UpdateAsync(updatedComment);

        return Ok(updatedComment);
    }

    // DELETE endpoint to delete a comment by ID
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteComment(int id)
    {
        var comment = await commentRepository.GetSingleAsync(id);

        if (comment is null)
        {
            return NotFound("Comment not found");
        }

        await commentRepository.DeleteAsync(id);

        return NoContent();
    }
}