using Entities;
using RepositoryContracts;

namespace CLI.UI.ManageUsers;

public class CreateUserView
{
    private readonly IUserRepository userRepository;

    public CreateUserView(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    public async Task StartAsync()
    {
        Console.Write("Enter username: ");
        string? username = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(username))
        {
            Console.WriteLine("Username cannot be empty.");
            return;
        }

        bool usernameExists = userRepository
            .GetMany()
            .Any(user => user.Username == username);

        if (usernameExists)
        {
            Console.WriteLine("Username is already taken.");
            return;
        }

        Console.Write("Enter password: ");
        string? password = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(password))
        {
            Console.WriteLine("Password cannot be empty.");
            return;
        }

        User user = new User(username, password);

        User createdUser = await userRepository.AddAsync(user);

        Console.WriteLine(
            $"User created successfully with ID: {createdUser.Id}");
    }
}