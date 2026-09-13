using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class ManagePostsView
{
    private readonly IUserRepository userRepository;
    private readonly IPostRepository postRepository;
    private readonly ICommentRepository commentRepository;

    public ManagePostsView(
        IUserRepository userRepository,
        IPostRepository postRepository,
        ICommentRepository commentRepository)
    {
        this.userRepository = userRepository;
        this.postRepository = postRepository;
        this.commentRepository = commentRepository;
    }

    public async Task StartAsync()
    {
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("MANAGE POSTS");
            Console.WriteLine("1. Create Post");
            Console.WriteLine("2. List Posts");
            Console.WriteLine("3. View Single Post");
            Console.WriteLine("0. Back");

            Console.Write("Choose an option: ");
            string? input = Console.ReadLine();

            if (input == "1")
            {
                CreatePostView createPostView =
                    new CreatePostView(postRepository, userRepository);

                await createPostView.StartAsync();
            }
            else if (input == "2")
            {
                ListPostsView listPostsView =
                    new ListPostsView(postRepository);

                await listPostsView.StartAsync();
            }
            else if (input == "3")
            {
                SinglePostView singlePostView =
                    new SinglePostView(
                        postRepository,
                        commentRepository,
                        userRepository
                    );

                await singlePostView.StartAsync();
            }
            else if (input == "0")
            {
                return;
            }
            else
            {
                Console.WriteLine("Invalid option.");
            }
        }
    }
}