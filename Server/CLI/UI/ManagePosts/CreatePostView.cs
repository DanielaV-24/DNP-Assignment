using Entities;
using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class CreatePostView
{
    private readonly IPostRepository postRepository;
    private readonly IUserRepository userRepository;

    public CreatePostView(
        IPostRepository postRepository,
        IUserRepository userRepository)
    {
        this.postRepository = postRepository;
        this.userRepository = userRepository;
    }

    public async Task StartAsync()
    {
        Console.Write("Enter title: ");
        string? title = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(title))
        {
            Console.WriteLine("Title cannot be empty.");
            return;
        }

        Console.Write("Enter body: ");
        string? body = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(body))
        {
            Console.WriteLine("Body cannot be empty.");
            return;
        }

        Console.Write("Enter user ID: ");
        string? userIdInput = Console.ReadLine();

        if (!int.TryParse(userIdInput, out int userId))
        {
            Console.WriteLine("User ID must be a number.");
            return;
        }

        bool userExists = userRepository
            .GetMany()
            .Any(user => user.Id == userId);

        if (!userExists)
        {
            Console.WriteLine("User does not exist.");
            return;
        }

        Post post = new Post(title, body, userId);

        Post createdPost = await postRepository.AddAsync(post);

        Console.WriteLine(
            $"Post created successfully with ID: {createdPost.Id}");
    }
}