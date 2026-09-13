namespace InMemoryRepositories;
using Entities;
using RepositoryContracts;

public class CommentInMemoryRepository : ICommentRepository
{
    private List<Comment> comments = new List<Comment>();
    
    public CommentInMemoryRepository()
    {
        // Post 1
        _ = AddAsync(new Comment("Definitely C#!", 1, 2)).Result;
        _ = AddAsync(new Comment("I really like Python.", 1, 3)).Result;
        _ = AddAsync(new Comment("Java is still my favorite.", 1, 4)).Result;

        // Post 2
        _ = AddAsync(new Comment("Good luck with your studies!", 2, 1)).Result;
        _ = AddAsync(new Comment("The first week is always a bit overwhelming.", 2, 3)).Result;
        _ = AddAsync(new Comment("You'll get used to it quickly!", 2, 5)).Result;

        // Post 3
        _ = AddAsync(new Comment("Pasta is always the answer!", 3, 1)).Result;
        _ = AddAsync(new Comment("You could make tacos.", 3, 2)).Result;
        _ = AddAsync(new Comment("Just order pizza. ", 3, 4)).Result;

        // Post 4
        _ = AddAsync(new Comment("I'm going hiking!", 4, 2)).Result;
        _ = AddAsync(new Comment("Probably just relaxing at home.", 4, 3)).Result;
        _ = AddAsync(new Comment("I'm visiting some friends.", 4, 1)).Result;

        // Post 5
        _ = AddAsync(new Comment("Async confused me at first too.", 5, 2)).Result;
        _ = AddAsync(new Comment("It gets easier with practice!", 5, 4)).Result;
        _ = AddAsync(new Comment("Just wait until you meet LINQ. ", 5, 5)).Result;

        // Post 6
        _ = AddAsync(new Comment("Interstellar!", 6, 1)).Result;
        _ = AddAsync(new Comment("Have you seen The Martian?", 6, 3)).Result;
        _ = AddAsync(new Comment("I'd recommend The Dark Knight.", 6, 4)).Result;
    }
    
    public Task<Comment> AddAsync(Comment comment)
    {
        comment.Id = comments.Any()
            ? comments.Max(c => c.Id) + 1
            : 1;
        comments.Add(comment);
        return Task.FromResult(comment);
    }

    public Task UpdateAsync(Comment comment)
    {
        Comment? existingComment = comments.SingleOrDefault(c => c.Id == comment.Id);
        if (existingComment is null)
        {
            throw new InvalidOperationException(
                $"Comment with ID '{comment.Id}' not found");
        }

        comments.Remove(existingComment);
        comments.Add(comment);
        
        return Task.FromResult(comment);
    }
    
    public Task DeleteAsync(int id)
    {
        Comment? commentToRemove = comments.SingleOrDefault(p => p.Id == id);
        if (commentToRemove is null)
        {
            throw new InvalidOperationException(
                $"Comment with ID '{id}' not found");
        }

        comments.Remove(commentToRemove);
        return Task.CompletedTask;
    }
    
    public Task<Comment> GetSingleAsync(int id)
    {
        Comment? comment = comments.SingleOrDefault(c => c.Id == id);
        if (comment is null)
        {
            throw new InvalidOperationException(
                $"Comment with ID '{id}' not found");
        }
        return Task.FromResult(comment);
    }
    
    public IQueryable<Comment> GetMany()
    {
        return comments.AsQueryable();
    }
}