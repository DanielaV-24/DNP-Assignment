using Entities;
using RepositoryContracts;

namespace CLI.UI.ManageUsers;

public class ListUsersView
{
    private readonly IUserRepository userRepository;

    public ListUsersView(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    public Task StartAsync()
    {
        IQueryable<User> users = userRepository.GetMany();

        Console.WriteLine();
        Console.WriteLine("USERS");

        foreach (User user in users)
        {
            Console.WriteLine(
                $"ID: {user.Id} - Username: {user.Username}");
        }

        return Task.CompletedTask;
    }
}