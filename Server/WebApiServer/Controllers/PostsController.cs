using ApiContracts;
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

    // GET: /Posts
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

        var result = posts.ToList().Select(post => new PostDto
        {
            Id = post.Id,
            Title = post.Title,
            Body = post.Body,
            UserId = post.UserId,
            Username = users
                .FirstOrDefault(u => u.Id == post.UserId)?.Username
        });

        return Ok(result);
    }

    // GET: /Posts/{id}
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetPostById(int id)
    {
        var post = await postRepository.GetSingleAsync(id);

        if (post is null)
        {
            return NotFound("Post not found");
        }

        var user = userRepository.GetMany()
            .FirstOrDefault(u => u.Id == post.UserId);

        var postDto = new PostDto
        {
            Id = post.Id,
            Title = post.Title,
            Body = post.Body,
            UserId = post.UserId,
            Username = user?.Username
        };

        return Ok(postDto);
    }

    // POST: /Posts
    [HttpPost]
    public async Task<IActionResult> CreatePost(
        [FromBody] CreatePostDto request)
    {
        var newPost = new Post(
            request.Title,
            request.Body,
            request.UserId);

        var createdPost = await postRepository.AddAsync(newPost);

        var postDto = new PostDto
        {
            Id = createdPost.Id,
            Title = createdPost.Title,
            Body = createdPost.Body,
            UserId = createdPost.UserId,
            Username = userRepository.GetMany()
                .FirstOrDefault(u => u.Id == createdPost.UserId)?.Username
        };

        return CreatedAtAction(
            nameof(GetPostById),
            new { id = createdPost.Id },
            postDto);
    }

    // PUT: /Posts/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdatePost(
        [FromRoute] int id,
        [FromBody] UpdatePostDto request)
    {
        var existingPost = await postRepository.GetSingleAsync(id);

        if (existingPost is null)
        {
            return NotFound("Post not found");
        }

        existingPost.Title = request.Title;
        existingPost.Body = request.Body;

        await postRepository.UpdateAsync(existingPost);

        var postDto = new PostDto
        {
            Id = existingPost.Id,
            Title = existingPost.Title,
            Body = existingPost.Body,
            UserId = existingPost.UserId,
            Username = userRepository.GetMany()
                .FirstOrDefault(u => u.Id == existingPost.UserId)?.Username
        };

        return Ok(postDto);
    }

    // DELETE: /Posts/{id}
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

    // GET: /Posts/{postId}/comments
    [HttpGet("{postId:int}/comments")]
    public IActionResult GetCommentsForPost(int postId)
    {
        var comments = commentRepository.GetMany()
            .Where(c => c.PostId == postId)
            .ToList();

        return Ok(comments);
    }
}
