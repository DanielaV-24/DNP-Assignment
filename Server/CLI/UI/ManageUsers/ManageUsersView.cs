using RepositoryContracts;

namespace CLI.UI.ManageUsers;

public class ManageUsersView
{
    private readonly IUserRepository userRepository;

    public ManageUsersView(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    public async Task StartAsync()
    {
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("MANAGE USERS");
            Console.WriteLine("1. Create User");
            Console.WriteLine("2. List Users");
            Console.WriteLine("0. Back");

            Console.Write("Choose an option: ");
            string? input = Console.ReadLine();

            if (input == "1")
            {
                CreateUserView createUserView =
                    new CreateUserView(userRepository);

                await createUserView.StartAsync();
            }
            else if (input == "2")
            {
                ListUsersView listUsersView =
                    new ListUsersView(userRepository);

                await listUsersView.StartAsync();
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