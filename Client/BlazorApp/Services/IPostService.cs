using ApiContracts;

namespace BlazorApp.Services;

public interface IPostService
{
    Task<IEnumerable<PostDto>> GetAllPostsAsync(
        string? title = null,
        int? userId = null,
        string? username = null);

    Task<PostDto?> GetPostByIdAsync(int id);

    Task<PostDto> AddPostAsync(CreatePostDto request);

    Task UpdatePostAsync(int id, UpdatePostDto request);

    Task DeletePostAsync(int id); 
}