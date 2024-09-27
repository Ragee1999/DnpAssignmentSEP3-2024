using Entities;
using RepositoryContracts;

namespace InMemoryRepository;

public class UserInMemoryRepository : IUserRepository
{
    public List<User> Users { get; set; }

    public Task<User> AddAsync(User user)
    {
        user.Id = Users.Any() 
            ? Users.Max(p => p.Id) + 1
            : 1;
        Users.Add(user);
        return Task.FromResult(user);
    }

    public Task UpdateAsync(User user)
    {
        User? existingPost = Users.SingleOrDefault(p => p.Id == user.Id);
        if (existingPost is null)
        {
            throw new InvalidOperationException(
                $"User with ID '{user.Id}' not found");
        }

        Users.Remove(existingPost);
        Users.Add(user);

        return Task.CompletedTask;
    }

    public Task DeleteAsync(int id)
    {
        User? userToRemove = Users.SingleOrDefault(p => p.Id == id);
        if (userToRemove is null)
        {
            throw new InvalidOperationException(
                $"User with ID '{id}' not found");
        }

        Users.Remove(userToRemove);
        return Task.CompletedTask;
    }

    public Task<User> GetSingleAsync(int id)
    {
        User? userToGet = Users.SingleOrDefault(p => p.Id == id);
        if (userToGet is null)
        {
            throw new InvalidOperationException(
                $"User with ID '{id}' not found");
        }
        return Task.FromResult(userToGet);
    }

    public IQueryable<User> GetMany()
    {
        return Users.AsQueryable();
    }
}