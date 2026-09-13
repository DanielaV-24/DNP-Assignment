using Entities;
using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class SinglePostView
{
    private readonly IPostRepository postRepository;
    private readonly ICommentRepository commentRepository;
    private readonly IUserRepository userRepository;

    public SinglePostView(
        IPostRepository postRepository,
        ICommentRepository commentRepository,
        IUserRepository userRepository)
    {
        this.postRepository = postRepository;
        this.commentRepository = commentRepository;
        this.userRepository = userRepository;
    }

    public async Task StartAsync()
    {
        Console.Write("Enter post ID: ");
        string? input = Console.ReadLine();

        if (!int.TryParse(input, out int postId))
        {
            Console.WriteLine("Post ID must be a number.");
            return;
        }

        Post post;

        try
        {
            post = await postRepository.GetSingleAsync(postId);
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine(ex.Message);
            return;
        }

        Console.WriteLine();
        Console.WriteLine($"Title: {post.Title}");
        Console.WriteLine($"Body: {post.Body}");
        Console.WriteLine($"User ID: {post.UserId}");

        var comments = commentRepository
            .GetMany()
            .Where(comment => comment.PostId == post.Id);

        Console.WriteLine();
        Console.WriteLine("COMMENTS");

        foreach (Comment comment in comments)
        {
            Console.WriteLine(
                $"User {comment.UserId}: {comment.Body}");
        }

        Console.WriteLine();
        Console.WriteLine("1. Add Comment");
        Console.WriteLine("0. Back");

        Console.Write("Choose an option: ");
        string? choice = Console.ReadLine();

        if (choice == "1")
        {
            await AddCommentAsync(post.Id);
        }
    }

    private async Task AddCommentAsync(int postId)
    {
        Console.Write("Enter user ID: ");
        string? userInput = Console.ReadLine();

        if (!int.TryParse(userInput, out int userId))
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

        Console.Write("Enter comment: ");
        string? body = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(body))
        {
            Console.WriteLine("Comment cannot be empty.");
            return;
        }

        Comment comment = new Comment(body, postId, userId);

        Comment createdComment =
            await commentRepository.AddAsync(comment);

        Console.WriteLine(
            $"Comment created successfully with ID: {createdComment.Id}");
    }
}