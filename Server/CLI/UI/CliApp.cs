using CLI.UI.ManagePosts;
using CLI.UI.ManageUsers;
using RepositoryContracts;

namespace CLI.UI;

public class CliApp
{
    private readonly IUserRepository userRepository;
    private readonly ICommentRepository commentRepository;
    private readonly IPostRepository postRepository;

    public CliApp(
        IUserRepository userRepository,
        ICommentRepository commentRepository,
        IPostRepository postRepository)
    {
        this.userRepository = userRepository;
        this.commentRepository = commentRepository;
        this.postRepository = postRepository;
    } 
    
    public async Task StartAsync()
    {
        while (true)
        {
            Console.WriteLine("MAIN MENU");
            Console.WriteLine("1. Manage Users");
            Console.WriteLine("2. Manage Posts");
            Console.WriteLine("0. Exit");

            Console.Write("Choose an option: ");
            string? input = Console.ReadLine();

            if (input == "1")
            {
                ManageUsersView manageUsersView =
                    new ManageUsersView(userRepository);

                await manageUsersView.StartAsync();
            }
            else if (input == "2")
            {
                ManagePostsView managePostsView =
                    new ManagePostsView(userRepository, postRepository, commentRepository);

                await managePostsView.StartAsync();
            }
            else if (input == "0")
            {
                return;
            }
            else
            {
                Console.WriteLine("Invalid option");
            }
        }
    }
}