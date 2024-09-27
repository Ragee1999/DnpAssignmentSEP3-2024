using Entities;
using RepositoryContracts;

namespace InMemoryRepository;

public class LikeInMemoryRepository : ILikeRepository
{
    public List<Like> Likes { get; set; }

    public Task<Like> AddAsync(Like like)
    {
        like.id = Likes.Any() 
            ? Likes.Max(p => p.id) + 1
            : 1;
        Likes.Add(like);
        return Task.FromResult(like);
    }

    public Task UpdateAsync(Like like)
    {
        Like? existingLike = Likes.SingleOrDefault(p => p.id == like.id);
        if (existingLike is null)
        {
            throw new InvalidOperationException(
                $"Like with ID '{like.id}' not found");
        }

        Likes.Remove(existingLike);
        Likes.Add(like);

        return Task.CompletedTask;
    }

    public Task DeleteAsync(int id)
    {
        Like? likeToRemove = Likes.SingleOrDefault(p => p.id == id);
        if (likeToRemove is null)
        {
            throw new InvalidOperationException(
                $"Like with ID '{id}' not found");
        }

        Likes.Remove(likeToRemove);
        return Task.CompletedTask;
    }

    public Task<Like> GetSingleAsync(int id)
    {
        Like? likeToGet = Likes.SingleOrDefault(p => p.id == id);
        if (likeToGet is null)
        {
            throw new InvalidOperationException(
                $"Like with ID '{id}' not found");
        }
        return Task.FromResult(likeToGet);
    }

    public IQueryable<Like> GetMany()
    {
        return Likes.AsQueryable();
    }
}