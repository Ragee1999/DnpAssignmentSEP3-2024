using Entities;
using RepositoryContracts;
using System.Text.Json;

namespace FileRepositories
{
    public class CommentFileRepository : ICommentRepository
    {
        private readonly string filePath = "comments.json";


        public CommentFileRepository()
        {
            if (!File.Exists(filePath))
            {
      
                File.WriteAllText(filePath, "[]");
            }
        }

        
        public async Task<Comment> AddAsync(Comment comment)
        {
  
            string commentsAsJson = await File.ReadAllTextAsync(filePath);
            List<Comment> comments = JsonSerializer.Deserialize<List<Comment>>(commentsAsJson)!;

   
            comment.Id = comments.Count > 0 ? comments.Max(c => c.Id) + 1 : 1;

     
            comments.Add(comment);


            commentsAsJson = JsonSerializer.Serialize(comments);
            await File.WriteAllTextAsync(filePath, commentsAsJson);


            return comment;
        }


        public async Task UpdateAsync(Comment comment)
        {

            string commentsAsJson = await File.ReadAllTextAsync(filePath);
            List<Comment> comments = JsonSerializer.Deserialize<List<Comment>>(commentsAsJson)!;


            var existingComment = comments.SingleOrDefault(c => c.Id == comment.Id);
            if (existingComment == null) 
                throw new InvalidOperationException($"Comment with ID {comment.Id} not found.");


            comments.Remove(existingComment);
            comments.Add(comment);


            commentsAsJson = JsonSerializer.Serialize(comments);
            await File.WriteAllTextAsync(filePath, commentsAsJson);
        }


        public async Task DeleteAsync(int id)
        {

            string commentsAsJson = await File.ReadAllTextAsync(filePath);
            List<Comment> comments = JsonSerializer.Deserialize<List<Comment>>(commentsAsJson)!;


            var commentToDelete = comments.SingleOrDefault(c => c.Id == id);
            if (commentToDelete == null) 
                throw new InvalidOperationException($"Comment with ID {id} not found.");

            comments.Remove(commentToDelete);


            commentsAsJson = JsonSerializer.Serialize(comments);
            await File.WriteAllTextAsync(filePath, commentsAsJson);
        }
        
        public async Task<Comment> GetSingleAsync(int id)
        {
            string commentsAsJson = await File.ReadAllTextAsync(filePath);
            List<Comment> comments = JsonSerializer.Deserialize<List<Comment>>(commentsAsJson)!;
            
            var comment = comments.SingleOrDefault(c => c.Id == id);
            if (comment == null) 
                throw new InvalidOperationException($"Comment with ID {id} not found.");

            return comment;
        }
        
        public IQueryable<Comment> GetMany()
        {
            string commentsAsJson = File.ReadAllTextAsync(filePath).Result;
            List<Comment> comments = JsonSerializer.Deserialize<List<Comment>>(commentsAsJson)!;
            
            return comments.AsQueryable();
        }
    }
}
