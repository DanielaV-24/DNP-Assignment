using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

    // GET: /Comments
    [HttpGet]
    public async Task<IActionResult> GetAllComments(
        [FromQuery] int? postId,
        [FromQuery] int? userId,
        [FromQuery] string? username)
    {
        var comments = commentRepository.GetMany();

        if (postId.HasValue)
            comments = comments.Where(c => c.PostId == postId.Value);

        if (userId.HasValue)
            comments = comments.Where(c => c.UserId == userId.Value);

        var users = await userRepository.GetMany().ToListAsync();

        if (!string.IsNullOrWhiteSpace(username))
        {
            var matchingUserIds = users
                .Where(u => u.Username.ToLower().Contains(username.ToLower()))
                .Select(u => u.Id)
                .ToList();

            comments = comments.Where(c => matchingUserIds.Contains(c.UserId));
        }

        var commentList = await comments.ToListAsync();

        var result = commentList.Select(c => new CommentDto
        {
            Id = c.Id,
            Body = c.Body,
            UserId = c.UserId,
            PostId = c.PostId,
            Username = users.FirstOrDefault(u => u.Id == c.UserId)?.Username
        });

        return Ok(result);
    }


    // GET: /Comments/{id}
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetCommentById(int id)
    {
        var comment = await commentRepository.GetSingleAsync(id);

        if (comment is null)
        {
            return NotFound("Comment not found");
        }

        var user = await userRepository.GetMany()
            .FirstOrDefaultAsync(u => u.Id == comment.UserId);

        var commentDto = new CommentDto
        {
            Id = comment.Id,
            Body = comment.Body,
            UserId = comment.UserId,
            PostId = comment.PostId,
            Username = user?.Username
        };

        return Ok(commentDto);
    }

    // POST: /Comments
    [HttpPost]
    public async Task<IActionResult> CreateComment(
        [FromBody] CreateCommentDto request)
    {
        var newComment = new Comment(
            request.Body,
            request.PostId,
            request.UserId);

        var createdComment = await commentRepository.AddAsync(newComment);

        var commentDto = new CommentDto
        {
            Id = createdComment.Id,
            Body = createdComment.Body,
            UserId = createdComment.UserId,
            PostId = createdComment.PostId,
            Username = (await userRepository.GetMany()
                .FirstOrDefaultAsync(u => u.Id == createdComment.UserId))?.Username
        };

        return CreatedAtAction(
            nameof(GetCommentById),
            new { id = createdComment.Id },
            commentDto);
    }

    // PUT: /Comments/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateComment(
        [FromRoute] int id,
        [FromBody] UpdateCommentDto request)
    {
        var existingComment = await commentRepository.GetSingleAsync(id);

        if (existingComment is null)
        {
            return NotFound("Comment not found");
        }

        existingComment.Body = request.Body;

        await commentRepository.UpdateAsync(existingComment);

        var commentDto = new CommentDto
        {
            Id = existingComment.Id,
            Body = existingComment.Body,
            UserId = existingComment.UserId,
            PostId = existingComment.PostId,
            Username = (await userRepository.GetMany()
                .FirstOrDefaultAsync(u => u.Id == existingComment.UserId))?.Username
        };

        return Ok(commentDto);
    }

    // DELETE: /Comments/{id}
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
