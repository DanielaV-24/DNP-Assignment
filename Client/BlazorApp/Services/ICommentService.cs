using ApiContracts;

namespace BlazorApp.Services;

public interface ICommentService
{
    Task<IEnumerable<CommentDto>> GetAllCommentsAsync(
        int? postId = null,
        int? userId = null,
        string? username = null);

    Task<CommentDto?> GetCommentByIdAsync(int id);

    Task<CommentDto> AddCommentAsync(CreateCommentDto request);

    Task UpdateCommentAsync(int id, UpdateCommentDto request);

    Task DeleteCommentAsync(int id);
}