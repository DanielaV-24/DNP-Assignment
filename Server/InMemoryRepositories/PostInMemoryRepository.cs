namespace InMemoryRepositories;
using Entities;
using RepositoryContracts;

public class PostInMemoryRepository : IPostRepository
{
    private List<Post> posts = new List<Post>();
    
    public PostInMemoryRepository()
    {
        _ = AddAsync(new Post("Best programming language?", "What programming language do you enjoy using the most?", 1)).Result;
        _ = AddAsync(new Post("First day at university", "Today was my first day at university. It was exciting but also a little overwhelming!", 2)).Result;
        _ = AddAsync(new Post("Easy dinner ideas", "I don't know what to cook tonight. Any quick suggestions?", 3)).Result;
        _ = AddAsync(new Post("Weekend plans", "Does anyone have something interesting planned for this weekend?", 4)).Result;
        _ = AddAsync(new Post("Learning C#", "I'm learning C# and async programming. Some parts are still confusing.", 1)).Result;
        _ = AddAsync(new Post("Movie recommendations", "I need a good movie to watch tonight. What would you recommend?", 5)).Result;
    }
    
    public Task<Post> AddAsync(Post post)
    {
        post.Id = posts.Any()
            ? posts.Max(p => p.Id) + 1
            : 1;
        posts.Add(post);
        return Task.FromResult(post);
    }
    
    public Task UpdateAsync(Post post)
    {
        Post? existingPost = posts.SingleOrDefault(p => p.Id == post.Id);
        if (existingPost is null)
        {
            throw new InvalidOperationException(
                $"Post with ID '{post.Id}' not found");
        }

        posts.Remove(existingPost);
        posts.Add(post);

        return Task.CompletedTask;
    }
    
    public Task DeleteAsync(int id)
    {
        Post? postToRemove = posts.SingleOrDefault(p => p.Id == id);
        if (postToRemove is null)
        {
            throw new InvalidOperationException(
                $"Post with ID '{id}' not found");
        }

        posts.Remove(postToRemove);
        return Task.CompletedTask;
    }
    
    public Task<Post> GetSingleAsync(int id)
    {
        Post? post = posts.SingleOrDefault(p => p.Id == id);
        if (post is null)
        {
            throw new InvalidOperationException(
                $"Post with ID '{id}' not found");
        }
        return Task.FromResult(post);
    }
    
    public IQueryable<Post> GetMany()
    {
        return posts.AsQueryable();
    }
}