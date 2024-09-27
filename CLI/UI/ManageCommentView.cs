using Entities;
using RepositoryContracts;

namespace CLI.UI
{
    public class ManageCommentView
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IPostRepository _postRepository;
        private readonly IUserRepository _userRepository;

        public ManageCommentView(ICommentRepository commentRepository, IPostRepository postRepository, IUserRepository userRepository)
        {
            _commentRepository = commentRepository;
            _postRepository = postRepository;
            _userRepository = userRepository;
        }

        public async Task ShowMenuAsync()
        {
            Console.Clear();
            Console.WriteLine("=== Manage Comments ===");
            Console.WriteLine("1. Add Comment to Post");
            Console.WriteLine("2. List Comments on a Post");
            Console.WriteLine("0. Back");

            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    await AddCommentAsync();
                    break;
                case "2":
                    await ListCommentsAsync();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Invalid choice, try again.");
                    break;
            }
        }

        private async Task AddCommentAsync()
        {
            Console.Write("Enter the Post ID to comment on: ");
            int postId;
            while (!int.TryParse(Console.ReadLine(), out postId))
            {
                Console.WriteLine("Invalid Post ID. Please enter a valid number.");
            }

            // Check if the post exists
            var post = await _postRepository.GetSingleAsync(postId);
            if (post == null)
            {
                Console.WriteLine($"Post with ID {postId} not found.");
                Console.ReadKey();
                return;
            }

            Console.Write("Enter your User ID: ");
            int userId;
            while (!int.TryParse(Console.ReadLine(), out userId))
            {
                Console.WriteLine("Invalid User ID. Please enter a valid number.");
            }

            // Checking if the user exist
            var user = await _userRepository.GetSingleAsync(userId);
            if (user == null)
            {
                Console.WriteLine($"User with ID {userId} not found.");
                Console.ReadKey();
                return;
            }

            Console.Write("Enter the comment body: ");
            string commentBody = Console.ReadLine();

            Comment newComment = new Comment
            {
                Body = commentBody,
                PostId = postId,
                UserId = userId
            };

            Comment createdComment = await _commentRepository.AddAsync(newComment);
            Console.WriteLine($"Comment added to Post ID {postId} with Comment ID: {createdComment.Id}");
            Console.ReadKey();
        }

        private async Task ListCommentsAsync()
        {
            Console.Write("Enter the Post ID to list comments for: ");
            int postId;
            while (!int.TryParse(Console.ReadLine(), out postId))
            {
                Console.WriteLine("Invalid Post ID. Please enter a valid number.");
            }

            // Checking if the post actually exist
            var post = await _postRepository.GetSingleAsync(postId);
            if (post == null)
            {
                Console.WriteLine($"Post with ID {postId} not found.");
                Console.ReadKey();
                return;
            }

            var comments = _commentRepository.GetMany().Where(c => c.PostId == postId);

            Console.WriteLine($"=== Comments for Post ID {postId} ===");
            foreach (var comment in comments)
            {
                var commentUser = await _userRepository.GetSingleAsync(comment.UserId);
                Console.WriteLine($"Comment ID: {comment.Id}, By User: {commentUser.UserName}");
                Console.WriteLine($"Comment Body: {comment.Body}");
                Console.WriteLine("--------------------------------------");
            }

            if (!comments.Any())
            {
                Console.WriteLine("No comments found for this post.");
            }

            Console.ReadKey();
        }
    }
}
