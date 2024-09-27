using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;



public class CommentInMemoryRepository : ICommentRepository
{
    public List<Comment> Comments { get; set; } = new List<Comment>();
    
    public CommentInMemoryRepository()
    {
        // SOME DUMMY DATA
        Comments.Add(new Comment { Id = 1, PostId = 1, UserId = 1, Body = "nice" });
        Comments.Add(new Comment { Id = 2, PostId = 2, UserId = 2, Body = "cool" });
    }

    public Task<Comment> AddAsync(Comment comment)
    {
        comment.Id = Comments.Any() 
            ? Comments.Max(p => p.Id) + 1
            : 1;
        Comments.Add(comment);
        return Task.FromResult(comment);
    }

    public Task UpdateAsync(Comment comment)
    {
        Comment? existingComment = Comments.SingleOrDefault(p => p.Id == comment.Id);
        if (existingComment is null)
        {
            throw new InvalidOperationException(
                $"Comment with ID '{comment.Id}' not found");
        }

        Comments.Remove(existingComment);
        Comments.Add(comment);

        return Task.CompletedTask;
    }

    public Task DeleteAsync(int id)
    {
        Comment? commentToRemove = Comments.SingleOrDefault(p => p.Id == id);
        if (commentToRemove is null)
        {
            throw new InvalidOperationException(
                $"Comment with ID '{id}' not found");
        }

        Comments.Remove(commentToRemove);
        return Task.CompletedTask;
    }

    public Task<Comment> GetSingleAsync(int id)
    {
        Comment? commentToGet = Comments.SingleOrDefault(p => p.Id == id);
        if (commentToGet is null)
        {
            throw new InvalidOperationException(
                $"Comment with ID '{id}' not found");
        }
        return Task.FromResult(commentToGet);
    }

    public IQueryable<Comment> GetMany()
    {
        return Comments.AsQueryable();
    }
}