using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;
using Entities;

namespace WebApiServer.Controllers;

[ApiController]
[Route("[controller]")]
public class PostsController : ControllerBase
{
    private readonly IPostRepository postRepository;
    private readonly ICommentRepository commentRepository;
    private readonly IUserRepository userRepository;

    public PostsController(
        IPostRepository postRepository,
        ICommentRepository commentRepository,
        IUserRepository userRepository)
    {
        this.postRepository = postRepository;
        this.commentRepository = commentRepository;
        this.userRepository = userRepository;
    }
    
    [HttpGet]
    public IActionResult GetAllPosts(
        [FromQuery] string? title,
        [FromQuery] int? userId,
        [FromQuery] string? username)
    {
        var posts = postRepository.GetMany();

        if (!string.IsNullOrWhiteSpace(title))
        {
            posts = posts.Where(p =>
                p.Title.Contains(
                    title,
                    StringComparison.OrdinalIgnoreCase));
        }

        if (userId.HasValue)
        {
            posts = posts.Where(p => p.UserId == userId.Value);
        }

        var users = userRepository.GetMany().ToList();

        if (!string.IsNullOrWhiteSpace(username))
        {
            var matchingUserIds = users
                .Where(u => u.Username.Contains(
                    username,
                    StringComparison.OrdinalIgnoreCase))
                .Select(u => u.Id)
                .ToList();

            posts = posts.Where(p =>
                matchingUserIds.Contains(p.UserId));
        }

        var result = posts.ToList().Select(post => new
        {
            post.Id,
            post.Title,
            post.Body,
            Username = users
                .FirstOrDefault(u => u.Id == post.UserId)?.Username
        });

        return Ok(result);
    }
    
    // GET endpoint to retrieve a specific post by ID
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetPostById(int id)
    {
        var post = await postRepository.GetSingleAsync(id);

        if (post is null)
        {
            return NotFound("Post not found");
        }

        return Ok(post);
    }
    
    // POST endpoint to add a new post
    [HttpPost]
    public async Task<IActionResult> CreatePost([FromBody] Post newPost)
    {
        var createdPost = await postRepository.AddAsync(newPost);

        return CreatedAtAction(
            nameof(GetPostById),
            new { id = createdPost.Id },
            createdPost);
    }
    
    // PUT endpoint to update an existing post
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdatePost(
        [FromRoute] int id,
        [FromBody] Post updatedPost)
    {
        if (id != updatedPost.Id)
        {
            return BadRequest("The ID in the URL does not match the post ID.");
        }

        await postRepository.UpdateAsync(updatedPost);

        return Ok(updatedPost);
    }
    
    // DELETE endpoint to delete a post by ID
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeletePost(int id)
    {
        var post = await postRepository.GetSingleAsync(id);

        if (post is null)
        {
            return NotFound("Post not found");
        }

        await postRepository.DeleteAsync(id);

        return NoContent();
    }
    
    // GET endpoint to retrieve comments belonging to a post
    [HttpGet("{postId:int}/comments")]
    public IActionResult GetCommentsForPost(int postId)
    {
        var comments = commentRepository.GetMany()
            .Where(c => c.PostId == postId)
            .ToList();

        return Ok(comments);
    }
}