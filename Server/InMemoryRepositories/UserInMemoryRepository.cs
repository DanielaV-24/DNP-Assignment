namespace InMemoryRepositories;
using Entities;
using RepositoryContracts;

public class UserInMemoryRepository : IUserRepository
{
    private List<User> users = new List<User>();
    
    public UserInMemoryRepository()
    {
        _ = AddAsync(new User("alice", "password123")).Result;
        _ = AddAsync(new User("bob", "hello123")).Result;
        _ = AddAsync(new User("charlie", "mypassword")).Result;
        _ = AddAsync(new User("diana", "diana456")).Result;
        _ = AddAsync(new User("emily", "secret789")).Result;
    }
    
    public Task<User> AddAsync(User user)
    {
        user.Id = users.Any()
            ? users.Max(u => u.Id) + 1
            : 1;
        users.Add(user);
        return Task.FromResult(user);
    }

    public Task UpdateAsync(User user)
    {
        User? existingUser = users.SingleOrDefault(u => u.Id == user.Id);
        if (existingUser is null)
        {
            throw new InvalidOperationException(
                $"User with ID '{user.Id}' not found");
        }

        users.Remove(existingUser);
        users.Add(user);
        
        return Task.FromResult(user);
    }
    
    public Task DeleteAsync(int id)
    {
        User? userToRemove = users.SingleOrDefault(p => p.Id == id);
        if (userToRemove is null)
        {
            throw new InvalidOperationException(
                $"User with ID '{id}' not found");
        }

        users.Remove(userToRemove);
        return Task.CompletedTask;
    }
    
    public Task<User> GetSingleAsync(int id)
    {
        User? user = users.SingleOrDefault(u => u.Id == id);
        if (user is null)
        {
            throw new InvalidOperationException(
                $"User with ID '{id}' not found");
        }
        return Task.FromResult(user);
    }
    
    public IQueryable<User> GetMany()
    {
        return users.AsQueryable();
    }
}