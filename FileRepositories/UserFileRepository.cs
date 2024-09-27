﻿using Entities;
using RepositoryContracts;
using System.Text.Json;

namespace FileRepositories
{
    public class UserFileRepository : IUserRepository
    {
        private readonly string filePath = "users.json";

        public UserFileRepository()
        {
            if (!File.Exists(filePath))
            {
                File.WriteAllText(filePath, "[]");
            }
        }

        public async Task<User> AddAsync(User user)
        {
            string usersAsJson = await File.ReadAllTextAsync(filePath);
            List<User> users = JsonSerializer.Deserialize<List<User>>(usersAsJson)!;

            user.Id = users.Count > 0 ? users.Max(u => u.Id) + 1 : 1;
            users.Add(user);

            usersAsJson = JsonSerializer.Serialize(users);
            await File.WriteAllTextAsync(filePath, usersAsJson);

            return user;
        }

        public async Task UpdateAsync(User user)
        {
            string usersAsJson = await File.ReadAllTextAsync(filePath);
            List<User> users = JsonSerializer.Deserialize<List<User>>(usersAsJson)!;

            var existingUser = users.SingleOrDefault(u => u.Id == user.Id);
            if (existingUser == null) throw new InvalidOperationException($"User with ID {user.Id} not found.");

            users.Remove(existingUser);
            users.Add(user);

            usersAsJson = JsonSerializer.Serialize(users);
            await File.WriteAllTextAsync(filePath, usersAsJson);
        }

        public async Task DeleteAsync(int id)
        {
            string usersAsJson = await File.ReadAllTextAsync(filePath);
            List<User> users = JsonSerializer.Deserialize<List<User>>(usersAsJson)!;

            var userToDelete = users.SingleOrDefault(u => u.Id == id);
            if (userToDelete == null) throw new InvalidOperationException($"User with ID {id} not found.");

            users.Remove(userToDelete);

            usersAsJson = JsonSerializer.Serialize(users);
            await File.WriteAllTextAsync(filePath, usersAsJson);
        }

        public async Task<User> GetSingleAsync(int id)
        {
            string usersAsJson = await File.ReadAllTextAsync(filePath);
            List<User> users = JsonSerializer.Deserialize<List<User>>(usersAsJson)!;

            var user = users.SingleOrDefault(u => u.Id == id);
            if (user == null) throw new InvalidOperationException($"User with ID {id} not found.");

            return user;
        }

        public IQueryable<User> GetMany()
        {
            string usersAsJson = File.ReadAllTextAsync(filePath).Result;
            List<User> users = JsonSerializer.Deserialize<List<User>>(usersAsJson)!;

            return users.AsQueryable();
        }
    }
}
