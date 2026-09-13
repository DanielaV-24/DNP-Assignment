using Entities;
using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class ListPostsView
{
    private readonly IPostRepository postRepository;

    public ListPostsView(IPostRepository postRepository)
    {
        this.postRepository = postRepository;
    }

    public Task StartAsync()
    {
        IQueryable<Post> posts = postRepository.GetMany();

        Console.WriteLine();
        Console.WriteLine("POSTS");

        foreach (Post post in posts)
        {
            Console.WriteLine(
                $"ID: {post.Id} | Title: {post.Title} | User ID: {post.UserId}");
        }

        return Task.CompletedTask;
    }
}